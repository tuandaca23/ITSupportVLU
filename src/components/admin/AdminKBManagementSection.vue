<template>
  <div class="text-black">
    <h1 class="text-3xl font-bold mb-6 text-black">Quản lý Bài viết hướng dẫn</h1>

    <!-- Custom File Upload -->
    <div class="mb-4 flex items-center gap-4">
      <input
        id="file-upload"
        type="file"
        accept=".xlsx,.csv"
        class="hidden"
        @change="handleFileChange"
      />
      <label
        for="file-upload"
        class="cursor-pointer px-4 py-2 bg-gray-200 text-black rounded hover:bg-gray-300 flex items-center gap-2">
        Chọn tệp
      </label>
      <span class="text-black">{{ fileName || 'Chưa chọn tệp' }}</span>
    </div>

    <button
      class="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700 mb-2 flex items-center gap-2"
    >
      <!-- Icon upload (mũi tên lên) -->
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v2a2 2 0 002 2h12a2 2 0 002-2v-2M12 12V4m0 0l-4 4m4-4l4 4"/>
      </svg>
      Nhập tệp
    </button>

    <p class="mt-4 text-slate-600">Tải lên file Excel/CSV để cập nhật bài viết hướng dẫn.</p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const fileName = ref('')

function handleFileChange(event: Event) {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    fileName.value = target.files[0].name
  } else {
    fileName.value = ''
  }
}
</script>
