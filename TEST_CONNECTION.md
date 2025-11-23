# Test Koneksi dan Verifikasi Aplikasi

## Apakah Aplikasi Benar-Benar Berfungsi?

**YA, aplikasi ini BUKAN hanya UI!** Ini adalah aplikasi lengkap dengan:

### 1. **Server Side (C#)**
- File: `bin/server.exe` atau `source/bin/Debug/server.exe`
- Menggunakan Microsoft Kinect SDK
- WebSocket server di `ws://127.0.0.1:8181`
- Mengirim data point cloud real-time dari Kinect

### 2. **Client Side (HTML/JavaScript)**
- File: `example/3d-scan.html`
- WebSocket client yang terhubung ke server
- Three.js untuk rendering 3D
- Processing dan export STL

## Cara Test Apakah Benar-Benar Berfungsi

### Step 1: Jalankan Server
```bash
# Buka terminal/command prompt
cd D:\informatika\webkinect\bin
server.exe
```

Atau double-click `server.exe` di folder `bin/`

**Yang harus muncul:**
- Console window dengan pesan "WebSocket server started"
- Jika Kinect terhubung: "Kinect sensor found"
- Jika tidak: "No kinect sensor found"

### Step 2: Buka Browser
1. Buka `example/3d-scan.html` di browser
2. Buka Developer Console (F12)

### Step 3: Verifikasi Koneksi

**Di Browser Console, cari log:**
```
🔗 Connecting to ws://127.0.0.1:8181...
✅ WebSocket connected successfully
📤 Sent PointCloud mode request
```

**Jika terhubung, akan muncul:**
```
✅ Point cloud data received: {pointCount: ..., width: ..., height: ...}
```

### Step 4: Test Rendering

1. Klik tombol **"🧪 Test Render"**
2. **Harus muncul sphere merah** di tengah viewer
3. Jika muncul = Three.js bekerja ✅

### Step 5: Verifikasi Data Masuk

**Di Console, cek:**
- Apakah ada log `✅ Point cloud data received`?
- Apakah `pointCount` > 0?
- Apakah ada `firstPoint` dengan nilai x, y, z?

**Di Debug Info Panel:**
- Connection Status: "Connected" (hijau)
- Last Message: "📦 Received X points"
- Data Received: angka bertambah

### Step 6: Lihat Point Cloud

Jika data masuk, harus terlihat:
- **Preview mode**: Points abu-abu transparan di viewer
- **Scanning mode**: Points berwarna yang terkumpul

## Troubleshooting

### Problem: Tidak ada data masuk

**Cek:**
1. ✅ Server.exe sudah berjalan?
2. ✅ Kinect hardware terhubung?
3. ✅ Kinect SDK terinstall?
4. ✅ WebSocket terhubung? (lihat status indicator)

**Test manual:**
```javascript
// Di browser console, test koneksi manual:
var testSocket = new WebSocket("ws://127.0.0.1:8181");
testSocket.onopen = () => console.log("✅ Connected!");
testSocket.onmessage = (e) => console.log("📦 Data:", e.data.substring(0, 100));
testSocket.onerror = (e) => console.error("❌ Error:", e);
```

### Problem: Data masuk tapi tidak terlihat

**Cek:**
1. ✅ Klik "🧪 Test Render" - apakah sphere muncul?
2. ✅ Cek console untuk error Three.js
3. ✅ Cek apakah points terfilter semua (ubah Min/Max Distance)
4. ✅ Cek camera position di console

**Test dengan data dummy:**
```javascript
// Di browser console:
var testData = {
    points: [
        {x: 0, y: 0, z: 1, r: 255, g: 0, b: 0},
        {x: 0.1, y: 0, z: 1, r: 0, g: 255, b: 0},
        {x: 0, y: 0.1, z: 1, r: 0, g: 0, b: 255}
    ],
    width: 640,
    height: 480
};
showPreview(testData);
```

## Verifikasi Lengkap

### ✅ Checklist:

- [ ] Server.exe berjalan tanpa error
- [ ] Kinect terdeteksi oleh server
- [ ] Browser terhubung ke WebSocket (status hijau)
- [ ] Data point cloud diterima (lihat console)
- [ ] Test Render menunjukkan sphere merah
- [ ] Point cloud terlihat di viewer
- [ ] Bisa generate mesh
- [ ] Bisa export STL

## Kesimpulan

**Aplikasi ini BENAR-BENAR berfungsi**, bukan hanya UI!

- **Server (C#)**: Mengambil data dari Kinect hardware
- **Client (HTML)**: Menerima data via WebSocket dan render 3D
- **Keduanya terhubung** via WebSocket protocol

Jika tidak ada data, kemungkinan:
1. Server belum dijalankan
2. Kinect tidak terhubung
3. Kinect SDK tidak terinstall
4. Firewall memblokir koneksi







