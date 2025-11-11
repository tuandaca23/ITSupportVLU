<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-6xl mx-auto bg-white p-8 rounded-lg shadow text-black">
      <!-- Header -->
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-black">Chi tiết Ticket: {{ ticketId }}</h1>
        <router-link
          to="/dashboard-ktv"
          class="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
        >
          Quay lại Dashboard
        </router-link>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- Chat History -->
        <div class="lg:col-span-2 bg-gray-50 p-4 rounded">
          <h2 class="text-xl font-semibold mb-4 text-black">Lịch sử Chat</h2>
          <div class="space-y-4 max-h-96 overflow-y-auto">
            <div v-for="(msg, index) in chatHistory" :key="index" 
                 :class="msg.isKtvMessage ? 'flex justify-end' : 'flex justify-start'">
              <div :class="msg.isKtvMessage ? 'bg-green-100' : 'bg-blue-100'" 
                   class="p-3 rounded-lg max-w-xs">
                <p class="text-sm font-semibold text-black">{{ msg.senderName }}</p>
                <p class="text-black">{{ msg.message }}</p>
                <span class="text-xs text-gray-500">{{ new Date(msg.timestamp).toLocaleString('vi-VN') }}</span>
              </div>
            </div>
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
          <button
            @click="updateStatus"
            class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 w-full"
          >
            Cập nhật Trạng thái
          </button>
        </div>
      </div>

      <!-- Actions -->
      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-4 text-black">Hành động</h2>
        <div class="flex gap-4 mb-4">
          <select class="p-2 border rounded text-black">
            <option>Chọn câu trả lời mẫu</option>
            <option>Câu 1: Đã tiếp nhận, đang xử lý.</option>
            <option>Câu 2: Vui lòng cung cấp thêm thông tin.</option>
            <option>Câu 3: Sự cố đã được khắc phục.</option>
          </select>
          <button class="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700">
            Chèn Câu trả lời
          </button>
        </div>
        <textarea
          v-model="message"
          rows="4"
          class="w-full p-3 border rounded text-black placeholder-gray-500"
          placeholder="Nhập tin nhắn..."
        ></textarea>
        <button
          @click="sendMessage"
          class="mt-4 bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700"
        >
          Gửi Tin nhắn
        </button>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { api } from '@/api/axios' // 1. Import helper API

const route = useRoute()
const ticketId = ref(route.params.id as string)
const status = ref('In Progress') // Giả định
const message = ref('')
const chatHistory = ref<any[]>([]) // 2. Biến mới cho lịch sử chat

// 3. Hàm tải lịch sử chat
async function fetchChatHistory() {
  try {
    const response = await api.get(`/tickets/${ticketId.value}/replies`)
    chatHistory.value = response.data
  } catch (error) {
    console.error('Lỗi tải lịch sử chat:', error)
  }
}

// 4. Tải lịch sử khi component được mounted
onMounted(fetchChatHistory)

async function sendMessage() {
  const ktvId = localStorage.getItem('currentUserId')
  if (!ktvId || message.value.trim() === '') return

  const payload = {
    userId: parseInt(ktvId),
    message: message.value,
  }

  try {
    // 5. Gọi API gửi tin nhắn
    await api.post(`/tickets/${ticketId.value}/replies`, payload)
    message.value = '' // Xóa tin nhắn đã gõ
    await fetchChatHistory() // 6. Tải lại lịch sử chat (Cách đơn giản)
    // (Sau này sẽ thay bằng SignalR để có real-time)
  } catch (error) {
    console.error('Lỗi gửi tin nhắn:', error)
  }
}

function updateStatus() {
  // TODO: Gọi API cập nhật trạng thái
  console.log(`Updated status for ticket ${ticketId.value} to ${status.value}`)
}
</script>

<style scoped>
/* Tailwind xử lý style chính */
</style>
