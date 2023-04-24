#include <deprecated.h>
#include <MFRC522.h>
#include <MFRC522Extended.h>
#include <require_cpp11.h>

#include <SPI.h>

#define RST_PIN   22   // RC522 modülünün reset pini
#define SS_PIN    21   // RC522 modülünün SS (slave select) pini


MFRC522 mfrc522(SS_PIN, RST_PIN);   // MFRC522 kütüphanesi ile RC522 modülünü tanımla




void setup() {
  Serial.begin(9600);   // Seri haberleşme başlat
  while (!Serial);      // Bilgisayara bağlantı sağlandığında devam et

  SPI.begin();          // SPI haberleşmesini başlat
  mfrc522.PCD_Init();   // RC522 modülünü başlat

}

void loop() {
  // RFID etiketi okunana kadar bekle
  if (!mfrc522.PICC_IsNewCardPresent() || !mfrc522.PICC_ReadCardSerial())
    return;
  String tagID = "";
  // Okunan etiketin ID'sini seri porta yazdır
  //Serial.print("Etiket ID'si: ");
  for (byte i = 0; i < mfrc522.uid.size; i++) {
    tagID.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? "0" : ""));
    tagID.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  Serial.println(tagID);
  //Serial.write(tagID.c_str(), tagID.length());
  
  mfrc522.PICC_HaltA(); // halt PICC
  mfrc522.PCD_StopCrypto1(); // stop encryption on PC
}
