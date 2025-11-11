<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-6xl mx-auto bg-white p-8 rounded-lg shadow text-black">
      <h1 class="text-3xl font-bold mb-6 text-black">Báo cáo</h1>

      <!-- Bộ lọc -->
      <div class="mb-6 flex gap-4 text-black">
        <input type="date" class="p-2 border rounded text-black placeholder-gray-500" />
        <input type="date" class="p-2 border rounded text-black placeholder-gray-500" />
        <select class="p-2 border rounded text-black">
          <option>Tất cả</option>
          <option>Lỗi Mạng</option>
          <option>Lỗi Phần mềm</option>
        </select>
        <select class="p-2 border rounded text-black">
          <option>Tất cả KTV</option>
          <option>KTV001</option>
        </select>
      </div>

      <!-- Biểu đồ -->
      <div class="mb-6 text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Biểu đồ Xu hướng</h2>
        <div class="h-64 bg-gray-200 rounded flex items-center justify-center text-black">[Placeholder] Line Chart</div>
      </div>
      <div class="mb-6 text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Biểu đồ Phân loại</h2>
        <div class="h-64 bg-gray-200 rounded flex items-center justify-center text-black">[Placeholder] Pie Chart</div>
      </div>
      <div class="mb-6 text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Biểu đồ Hiệu suất</h2>
        <div class="h-64 bg-gray-200 rounded flex items-center justify-center text-black">[Placeholder] Bar Chart</div>
      </div>

      <!-- Nút Xuất Excel -->
      <button class="bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700">Xuất Excel</button>
    </div>
  </main>
</template>

<script setup lang="ts">
// Không có logic
</script>

<style scoped>
/* Tailwind xử lý phần lớn, text-black đảm bảo chữ luôn hiển thị đậm */
</style>
