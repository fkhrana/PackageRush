#include <LiquidCrystal.h>

// Pin configuration
const int buttonLeft = 2;
const int buttonRight = 3;
const int buttonJump = 4;
const int buttonP2Left = 5;
const int buttonP2Right = 6;
const int buttonP2Jump = 7;
const int buzzerPin = 8;
const int winLedPin = 9;
const int lcdRs = 10;
const int lcdEnable = 11;
const int lcdD4 = 12;
const int lcdD5 = 13;
const int lcdD6 = A0;
const int lcdD7 = A1;
LiquidCrystal lcd(lcdRs, lcdEnable, lcdD4, lcdD5, lcdD6, lcdD7);

bool lastJumpState = HIGH;
bool lastJump2State = HIGH;
String currentCommand = "";

void setup() {
  pinMode(buttonLeft, INPUT_PULLUP);
  pinMode(buttonRight, INPUT_PULLUP);
  pinMode(buttonJump, INPUT_PULLUP);
  pinMode(buttonP2Left, INPUT_PULLUP);
  pinMode(buttonP2Right, INPUT_PULLUP);
  pinMode(buttonP2Jump, INPUT_PULLUP);
  pinMode(buzzerPin, OUTPUT);
  pinMode(winLedPin, OUTPUT);
  digitalWrite(buzzerPin, LOW);
  digitalWrite(winLedPin, LOW);

  Serial.begin(9600);
  lcd.begin(16, 2);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Score: 0");
  lcd.setCursor(0, 1);
  lcd.print("Ready");
}

void loop() {
  processSerial();
  sendButtonCommands();
  delay(20);
}

void processSerial() {
  while (Serial.available()) {
    char c = Serial.read();
    if (c == '\n' || c == '\r') {
      if (currentCommand.length() > 0) {
        processCommand(currentCommand);
        currentCommand = "";
      }
    } else if (c >= 32) {
      currentCommand += c;
    }
  }
}

void sendButtonCommands() {
  bool leftPressed = digitalRead(buttonLeft) == LOW;
  bool rightPressed = digitalRead(buttonRight) == LOW;
  bool jumpPressed = digitalRead(buttonJump) == LOW;
  bool p2LeftPressed = digitalRead(buttonP2Left) == LOW;
  bool p2RightPressed = digitalRead(buttonP2Right) == LOW;
  bool p2JumpPressed = digitalRead(buttonP2Jump) == LOW;

  if (leftPressed && !rightPressed) {
    Serial.println("LEFT");
  } else if (rightPressed && !leftPressed) {
    Serial.println("RIGHT");
  } else {
    Serial.println("STOP");
  }

  if (jumpPressed && lastJumpState == HIGH) {
    Serial.println("JUMP");
  }

  if (p2LeftPressed && !p2RightPressed) {
    Serial.println("P2LEFT");
  } else if (p2RightPressed && !p2LeftPressed) {
    Serial.println("P2RIGHT");
  } else {
    Serial.println("P2STOP");
  }

  if (p2JumpPressed && lastJump2State == HIGH) {
    Serial.println("P2JUMP");
  }

  lastJumpState = jumpPressed ? LOW : HIGH;
  lastJump2State = p2JumpPressed ? LOW : HIGH;
}

void processCommand(String cmd) {
  if (cmd.equalsIgnoreCase("GAMEOVER")) {
    showGameOver();
  } else if (cmd.equalsIgnoreCase("WIN")) {
    showWin();
  } else if (cmd.startsWith("SCORE:")) {
    String value = cmd.substring(6);
    showScore(value);
  } else if (cmd.equalsIgnoreCase("RESET")) {
    resetDisplay();
  }
}

void showGameOver() {
  digitalWrite(winLedPin, LOW);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("GAME OVER");
  lcd.setCursor(0, 1);
  lcd.print("Try Again");
  tone(buzzerPin, 1000, 500);
  delay(600);
  noTone(buzzerPin);
}

void showWin() {
  digitalWrite(winLedPin, HIGH);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("YOU WIN!");
  lcd.setCursor(0, 1);
  lcd.print("Score Saved");
}

void showScore(String scoreText) {
  lcd.setCursor(0, 0);
  lcd.print("Score:");
  lcd.print("        ");
  lcd.setCursor(7, 0);
  lcd.print(scoreText);
  lcd.setCursor(0, 1);
  lcd.print("Collecting...");
}

void resetDisplay() {
  digitalWrite(winLedPin, LOW);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Score: 0");
  lcd.setCursor(0, 1);
  lcd.print("Ready");
}
