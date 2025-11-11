<template>
  <div>
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

    <!-- Nút Xuất Excel với icon download -->
    <button
      class="bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700 flex items-center gap-2"
    >
      <!-- Icon download -->
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v2a2 2 0 002 2h12a2 2 0 002-2v-2M12 4v12m0 0l-4-4m4 4l4-4"/>
      </svg>
      Xuất Excel
    </button>
  </div>
</template>

<script setup lang="ts">
// Không có logic
</script>
