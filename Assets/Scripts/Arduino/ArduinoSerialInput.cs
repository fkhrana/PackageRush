using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoSerialInput : MonoBehaviour
{
    [Header("Serial Settings")]
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private float reconnectDelay = 2f;
    [SerializeField] private bool autoConnect = true;

    [Header("Target")]
    [SerializeField] private PlayerController2D playerController;

    private SerialPort serialPort;
    private Thread readThread;
    private readonly ConcurrentQueue<string> commandQueue = new ConcurrentQueue<string>();
    private readonly object portLock = new object();
    private volatile bool isRunning;
    private float nextReconnectTime;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController2D>();
        }

        if (playerController == null)
        {
            Debug.LogError("ArduinoSerialInput: PlayerController2D tidak ditemukan.", this);
            enabled = false;
            return;
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
        if (playerController == null)
        {
            return;
        }

        switch (command.ToUpperInvariant())
        {
            case "L":
            case "LEFT":
                playerController.MoveLeft();
                break;
            case "R":
            case "RIGHT":
                playerController.MoveRight();
                break;
            case "S":
            case "STOP":
                playerController.StopMove();
                break;
            case "J":
            case "JUMP":
                playerController.Jump();
                break;
        }
    }

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