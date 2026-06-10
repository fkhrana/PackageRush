using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoSerialInput : MonoBehaviour
{
    public static ArduinoSerialInput Instance { get; private set; }

    [Header("Serial Settings")]
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private float reconnectDelay = 2f;
    [SerializeField] private bool autoConnect = true;

    [Header("Target")]
    [SerializeField] private PlayerController2D player1Controller;
    [SerializeField] private PlayerController2D player2Controller;

    private SerialPort serialPort;
    private Thread readThread;
    private readonly ConcurrentQueue<string> commandQueue = new ConcurrentQueue<string>();
    private readonly object portLock = new object();
    private volatile bool isRunning;
    private float nextReconnectTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("Duplicate ArduinoSerialInput instance found, destroying duplicate.", this);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player1Controller == null || player2Controller == null)
        {
            PlayerController2D[] controllers = FindObjectsByType<PlayerController2D>(FindObjectsSortMode.None);
            foreach (PlayerController2D controller in controllers)
            {
                if (controller == null)
                {
                    continue;
                }

                if (controller.PlayerNumber == 1 && player1Controller == null)
                {
                    player1Controller = controller;
                }
                else if (controller.PlayerNumber == 2 && player2Controller == null)
                {
                    player2Controller = controller;
                }
            }
        }

        if (player1Controller == null)
        {
            player1Controller = FindFirstObjectByType<PlayerController2D>();
        }

        if (player1Controller == null)
        {
            Debug.LogError("ArduinoSerialInput: PlayerController2D untuk Player 1 tidak ditemukan.", this);
            enabled = false;
            return;
        }

        if (player2Controller == null)
        {
            Debug.LogWarning("ArduinoSerialInput: PlayerController2D untuk Player 2 tidak ditemukan. Hanya Player 1 akan dikontrol oleh Arduino.", this);
        }

        if (autoConnect)
        {
            TryConnect();
        }
    }

    private void Update()
    {
        while (commandQueue.TryDequeue(out string command))
        {
            HandleCommand(command);
        }

        if (!autoConnect || IsPortOpen())
        {
            return;
        }

        if (Time.unscaledTime >= nextReconnectTime)
        {
            nextReconnectTime = Time.unscaledTime + reconnectDelay;
            TryConnect();
        }
    }

    private void TryConnect()
    {
        lock (portLock)
        {
            if (isRunning)
            {
                return;
            }

            try
            {
                serialPort = new SerialPort(portName, baudRate)
                {
                    NewLine = "\n",
                    ReadTimeout = 500,
                    DtrEnable = true,
                    RtsEnable = true
                };

                serialPort.Open();
                isRunning = true;
                readThread = new Thread(ReadLoop)
                {
                    IsBackground = true
                };
                readThread.Start();

                Debug.Log($"ArduinoSerialInput connected to {portName} at {baudRate} baud.", this);
            }
            catch (Exception exception)
            {
                ClosePort();
                Debug.LogError($"ArduinoSerialInput gagal membuka {portName}: {exception.Message}", this);
            }
        }
    }

    private void ReadLoop()
    {
        while (isRunning)
        {
            try
            {
                string line = serialPort.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    commandQueue.Enqueue(line.Trim());
                }
            }
            catch (TimeoutException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"ArduinoSerialInput read error: {exception.Message}", this);
                break;
            }
        }

        isRunning = false;
    }

    private void HandleCommand(string command)
    {
        switch (command.ToUpperInvariant())
        {
            case "L":
            case "LEFT":
            case "P1LEFT":
                player1Controller?.MoveLeft();
                break;
            case "R":
            case "RIGHT":
            case "P1RIGHT":
                player1Controller?.MoveRight();
                break;
            case "S":
            case "STOP":
            case "P1STOP":
                player1Controller?.StopMove();
                break;
            case "J":
            case "JUMP":
            case "P1JUMP":
                player1Controller?.Jump();
                break;
            case "P2LEFT":
                player2Controller?.MoveLeft();
                break;
            case "P2RIGHT":
                player2Controller?.MoveRight();
                break;
            case "P2STOP":
                player2Controller?.StopMove();
                break;
            case "P2JUMP":
                player2Controller?.Jump();
                break;
        }
    }

    public bool SendCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return false;
        }

        lock (portLock)
        {
            if (!IsPortOpen())
            {
                return false;
            }

            try
            {
                serialPort.WriteLine(command);
                Debug.Log($"ArduinoSerialInput send: {command}", this);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"ArduinoSerialInput write error: {exception.Message}", this);
                return false;
            }
        }
    }

    public void SendGameOver() => SendCommand("GAMEOVER");
    public void SendWin() => SendCommand("WIN");
    public void SendScore(int score) => SendCommand($"SCORE:{score}");
    public void SendReset() => SendCommand("RESET");

    private bool IsPortOpen()
    {
        lock (portLock)
        {
            return serialPort != null && serialPort.IsOpen && isRunning;
        }
    }

    private void OnApplicationQuit()
    {
        ClosePort();
    }

    private void OnDisable()
    {
        ClosePort();
    }

    private void ClosePort()
    {
        lock (portLock)
        {
            isRunning = false;

            if (readThread != null && readThread.IsAlive)
            {
                try
                {
                    readThread.Join(100);
                }
                catch (Exception)
                {
                }
            }

            if (serialPort != null)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"ArduinoSerialInput close error: {exception.Message}", this);
                }
                finally
                {
                    serialPort.Dispose();
                    serialPort = null;
                }
            }

            readThread = null;
        }
    }
}