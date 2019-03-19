int rightSensorPin = 3;
int leftSensorPin = 0;
int leftSensorValue = 0;
int rightSensorValue = 0;


void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.flush();
}

void print (String message){
  Serial.print(message);
  }
void print (int message){
  Serial.print(message);
  }

void loop() {
  // put your main code here, to run repeatedly:
  leftSensorValue = analogRead(leftSensorPin);
  rightSensorValue = analogRead(rightSensorPin);
  print("R:");
  print(rightSensorValue);
  print("|L:");
  print(leftSensorValue);
  Serial.println();
  
  delay(500);
}
