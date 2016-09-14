int POTY_PIN = 2;
int button = 6;

void setup() {
  Serial.begin(9600);
  pinMode(button, INPUT_PULLUP);
}

void loop() {
  int val = analogRead(POTY_PIN);
  int val2 = val/256;
  int val3 = val%256;
  int val4 = digitalRead(button);
  Serial.print(val);
  Serial.write(val2);
  Serial.write(val3);
  Serial.write(val4);
  delay(100);
}
