# Project Frisbee

Bu proje, Unity ile geliştirilen bir 3D **Frisbee kontrol oyunu**dur. Oyuncu bir frisbee'yi WASD tuşlarıyla kontrol eder, çeşitli yeteneklerle engellerden kaçar ve enerji halkalarından geçerek ilerler.

## Oynanış Özellikleri

- WASD tuşlarıyla frisbee'yi yönlendir.
- Space ile yukarı, Ctrl ile aşağı hareket.
- Dash (hızlı atılma) özelliği ile engellerden kaç.
- Flip (takla) hareketiyle çevik manevralar yap.
- Enerji halkalarından geçerek enerji topla.
- Enerji sıfırlandığında oyun sona erer.

## Teknik Özellikler

- Unity fizik sistemi kullanılarak gerçekçi frisbee hareketleri simüle edildi.
- Kod yapısı: C# scriptler üzerinden modular tasarlandı.
- Oyun içi enerji sistemi ve UI ile etkileşimli gösterge.
- Gelişmiş kamera takibi: Flip sırasında yumuşak geçiş.

## Klasör Yapısı

```plaintext
Assets/
 ├── Scripts/             # Oyun mekanikleri için C# kodları
 ├── Scenes/              # Oyun seviyeleri
 ├── Prefabs/             # Hazır nesneler
 ├── Materials/           # Materyal ve shader dosyaları
