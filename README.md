# 👨‍💼 Employee Management API

Sebuah proyek **Employee Management** berbasis .NET Core Web API untuk mengelola data karyawan, otentikasi pengguna, dan pengelompokan berdasarkan kelas dan status.

---

## 📦 Struktur Data

Proyek ini memiliki beberapa entitas utama:

### 1. **Employee**
Data utama karyawan, berisi informasi:
- `Nama`
- `Salary`
- `Age`
- `Class` (mengacu ke tabel `Class`)
- `Status` (mengacu ke tabel `Status`)
- `IsDeleted` (soft delete flag)

### 2. **Auth**
Tabel autentikasi pengguna:
- `Username`
- `Password`
- `EmployeeId` (foreign key ke tabel `Employee`)
- `AccessLevel`: menentukan hak akses pengguna (`User`, `Admin`, `Superuser`)

### 3. **Class**
Tabel yang mendefinisikan tipe/kategori karyawan.  
Contoh: `Intern`, `Staff`, `Manager`

### 4. **Status**
Tabel status kerja karyawan.  
Contoh: `Active`, `On Leave`, `Resigned`

---

## 🔐 Login Flow

1. User login menggunakan **username** dan **password** dari tabel `Auth`.
2. Jika sukses, sistem mengembalikan **token** (JWT).
3. Token menyimpan informasi akses: `AccessLevel`.
4. Sistem menyesuaikan fitur berdasarkan level akses:
   - `User`: hanya bisa melihat data
   - `Admin`: bisa tambah/edit/hapus data serta membuat akun dengan level `User`
   - `Superuser`: akses penuh termasuk membuat akun dengan level `Admin` dan `User`

---

## 🧠 Redis Cache

Proyek ini mendukung **Redis** sebagai cache untuk meningkatkan performa, terutama pada data yang sering diakses
Pastikan Docker terinstall untuk menjalankan proyek ini.

- Caching data employee untuk mengurangi query DB berulang

### 🔧 Cara Setup Redis:

1. **Install Redis** (local atau pakai container Docker):
   ```bash
   docker-compose run
