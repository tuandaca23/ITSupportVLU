<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-4xl mx-auto bg-white p-8 rounded-lg shadow text-black">
      <h1 class="text-2xl font-bold mb-6 text-black">
        Chi tiết Yêu cầu: [Placeholder] Tiêu đề
      </h1>

      <p class="text-black"><strong>Trạng thái:</strong> [Placeholder] Mới</p>
      <p class="text-black"><strong>KTV phụ trách:</strong> [Placeholder] KTV001</p>

      <!-- Lịch sử trao đổi -->
      <div class="mb-6 text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Lịch sử trao đổi</h2>
        <div class="mb-4 p-4 bg-gray-100 rounded text-black">
          <p><strong>[Placeholder] KTV001:</strong> [Placeholder] Tin nhắn</p>
          <p class="text-sm text-gray-600">[Placeholder] 2023-10-02</p>
        </div>
        <!-- Có thể thêm các div lịch sử khác -->
      </div>

      <!-- Khung phản hồi -->
      <form>
        <textarea
          placeholder="Gửi phản hồi..."
          rows="4"
          class="w-full p-3 border rounded mb-4 text-black placeholder-gray-500"
        ></textarea>
        <button
          type="submit"
          class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700 mr-4"
        >
          Gửi
        </button>
        <button
          type="button"
          @click="goBack"
          class="bg-red-600 text-white px-6 py-3 rounded hover:bg-red-700"
        >
          Đóng yêu cầu
        </button>
      </form>
    </div>
  </main>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
const router = useRouter()
const goBack = () => {
  router.back()
}
</script>

<style scoped>
/* Tailwind xử lý phần giao diện; chỉ cần text-black cho toàn trang */
</style>
