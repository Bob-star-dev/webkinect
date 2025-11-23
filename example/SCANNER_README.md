# Kinect 3D Scanner - Depth to STL

Aplikasi lengkap untuk mengubah data depth dari Kinect menjadi point cloud 3D, lalu mesh, dan export ke format STL.

## 📁 File Structure

```
example/
├── scanner.html          # HTML utama dengan UI
├── kinect-scanner.js     # JavaScript class utama
└── SCANNER_README.md     # Dokumentasi ini
```

## 🚀 Cara Menggunakan

### 1. Persiapan

1. **Jalankan WebKinect Server:**
   ```bash
   cd bin
   server.exe
   ```
   Atau double-click `bin/server.exe`

2. **Pastikan Kinect terhubung** ke PC dan terdeteksi oleh server

### 2. Buka Aplikasi

1. Buka file `example/scanner.html` di browser
2. Aplikasi akan otomatis connect ke server

### 3. Workflow

1. **Connect** - Terhubung ke WebKinect server
2. **Start Scan** - Capture satu frame depth data
3. **Generate Mesh** - Buat 3D mesh dari point cloud
4. **Export STL** - Download file .stl

## 🔧 Fitur

### Core Features
- ✅ Depth → Point Cloud conversion
- ✅ Point Cloud → Mesh triangulation
- ✅ Three.js 3D visualization
- ✅ STL export

### Advanced Features
- ✅ Mesh smoothing (Laplacian smoothing)
- ✅ Configurable depth range
- ✅ Point skipping untuk performa
- ✅ Adjustable mesh resolution
- ✅ Interactive 3D viewer (drag to rotate, scroll to zoom)

## ⚙️ Settings

### Min/Max Depth
- Range: 0.5 - 4.0 meters
- Default: 0.85 - 4.0m
- Filter points berdasarkan jarak dari sensor

### Point Skip
- Range: 1 - 10
- Default: 2
- Skip setiap N points untuk performa
- Semakin besar = lebih cepat tapi kurang detail

### Mesh Resolution
- Range: 0.01 - 0.1 meters
- Default: 0.05m
- Ukuran grid untuk triangulasi
- Semakin kecil = lebih detail tapi lebih lambat

### Options
- **Smoothing**: Aktifkan Laplacian smoothing untuk mesh lebih halus
- **Hole Filling**: (Coming soon) Isi lubang di mesh

## 📊 Algoritma

### 1. Depth to Point Cloud
- Server mengirim data point cloud langsung (sudah dikonversi)
- Filter berdasarkan Z coordinate (jarak dari sensor)
- Apply point skipping untuk performa

### 2. Point Cloud to Mesh
- **Grid-based Triangulation:**
  - Buat spatial hash grid
  - Untuk setiap point, cari neighbors dalam radius
  - Buat triangles dari 3 nearby points
  - Validasi triangle quality

### 3. Mesh Smoothing
- **Laplacian Smoothing:**
  - Build adjacency list
  - Average position dengan neighbors
  - Blend original dengan average (50/50)

### 4. STL Export
- Convert Three.js geometry ke STL ASCII format
- Calculate normals untuk setiap triangle
- Format sesuai STL standard

## 🐛 Troubleshooting

### Tidak ada data masuk
- ✅ Pastikan server.exe berjalan
- ✅ Pastikan Kinect terhubung
- ✅ Cek console browser untuk error

### Points tidak terlihat
- ✅ Cek Min/Max Depth settings
- ✅ Coba ubah Point Skip ke 1
- ✅ Lihat console untuk jumlah points

### Mesh tidak ter-generate
- ✅ Pastikan ada cukup points (>100)
- ✅ Coba ubah Mesh Resolution
- ✅ Cek console untuk error

### STL tidak bisa dibuka
- ✅ Pastikan mesh sudah di-generate
- ✅ Coba buka di software 3D lain (Blender, MeshLab)
- ✅ Cek ukuran file (tidak boleh 0 bytes)

## 📝 Code Structure

### `Kinect3DScanner` Class

```javascript
// Main methods:
- initThreeJS(containerId)      // Initialize Three.js
- connect()                      // Connect to WebSocket
- startScan()                    // Capture one frame
- generateMesh()                 // Create mesh from points
- exportSTL()                   // Export to STL file

// Helper methods:
- processPointCloudData(data)    // Process incoming data
- displayPointCloud(points)      // Render points
- triangulate(points)            // Create triangles
- smoothGeometry(geometry)       // Smooth mesh
- convertToSTL(geometry)        // STL conversion
```

## 🎯 Next Steps / Improvements

### Possible Enhancements:
1. **Real-time scanning** - Multiple frames accumulation
2. **Better triangulation** - Delaunay triangulation
3. **Hole filling** - Fill gaps in mesh
4. **Mesh simplification** - Reduce triangle count
5. **Texture mapping** - Add color from Kinect color camera
6. **Multiple export formats** - OBJ, PLY, GLTF
7. **Cloud processing** - Upload to cloud for processing

## 📚 Dependencies

- **Three.js** (r128) - 3D rendering
- **WebKinect Server** - Kinect data source
- **Modern Browser** - WebSocket support

## 📄 License

Same as WebKinect repository (MIT)

## 🙏 Credits

- Based on [WebKinect](https://github.com/tentone/webkinect)
- Three.js by [mrdoob](https://github.com/mrdoob/three.js)







