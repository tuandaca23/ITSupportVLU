<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-6xl mx-auto bg-white p-8 rounded-lg shadow text-black">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-black">Chi tiết Ticket: {{ ticketId }}</h1>
        <router-link to="/dashboard-admin" class="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700">Quay lại Dashboard</router-link>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- Chat History -->
        <div class="lg:col-span-2 bg-gray-50 p-4 rounded">
          <h2 class="text-xl font-semibold mb-4 text-black">Lịch sử Chat</h2>
          <div class="space-y-4 max-h-96 overflow-y-auto">
            <!-- Mock messages -->
            <div class="flex justify-start">
              <div class="bg-blue-100 p-3 rounded-lg max-w-xs">
                <p class="text-black">Sinh viên: Lỗi mạng không kết nối được.</p>
                <span class="text-xs text-gray-500">2023-10-01 10:00</span>
              </div>
            </div>
            <div class="flex justify-end">
              <div class="bg-green-100 p-3 rounded-lg max-w-xs">
                <p class="text-black">Admin: Đã kiểm tra, vui lòng thử lại.</p>
                <span class="text-xs text-gray-500">2023-10-01 10:15</span>
              </div>
            </div>
            <!-- Add more mock messages -->
          </div>
        </div>

        <!-- Properties Panel -->
        <div class="bg-gray-50 p-4 rounded">
          <h2 class="text-xl font-semibold mb-4 text-black">Thuộc tính</h2>
          <div class="mb-4">
            <label class="block text-sm font-medium mb-2 text-black">Trạng thái</label>
            <select v-model="status" class="w-full p-2 border rounded text-black">
              <option>New</option>
              <option>In Progress</option>
              <option>Resolved</option>
            </select>
          </div>
          <button @click="updateStatus" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 w-full">Cập nhật Trạng thái</button>
        </div>
      </div>

      <!-- Actions -->
      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-4 text-black">Hành động</h2>
        <div class="flex gap-4 mb-4">
          <select class="p-2 border rounded text-black">
            <option>Chọn câu trả lời mẫu</option>
            <option>Câu 1: Vui lòng thử lại sau.</option>
            <option>Câu 2: Đã khắc phục.</option>
          </select>
          <button class="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700">Chèn Câu trả lời</button>
        </div>
        <textarea v-model="message" rows="4" class="w-full p-3 border rounded text-black placeholder-gray-500" placeholder="Nhập tin nhắn..."></textarea>
        <button @click="sendMessage" class="mt-4 bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700">Gửi Tin nhắn</button>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const ticketId = ref(route.params.id as string)
const status = ref('New')
const message = ref('')

function updateStatus() {
  console.log(`Updated status for ticket ${ticketId.value} to ${status.value}`)
  // Mock: Update via API
}

function sendMessage() {
  console.log(`Sent message: ${message.value} for ticket ${ticketId.value}`)
  // Mock: Send message via API, then clear
  message.value = ''
}
</script>

<style scoped>
/* Tailwind handles styling */
</style>