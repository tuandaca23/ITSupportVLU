<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-6xl mx-auto bg-white p-8 rounded-lg shadow text-black" v-if="ticket">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-black">Chi tiết Ticket: {{ ticket.title }}</h1>
        <router-link
          to="/dashboard-ktv"
          class="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
        >
          Quay lại Dashboard
        </router-link>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div class="lg:col-span-2 bg-gray-50 p-4 rounded">
          <h2 class="text-xl font-semibold mb-4 text-black">Lịch sử Chat</h2>
          <div class="space-y-4 max-h-96 overflow-y-auto">
            
            <div v-for="(msg, index) in chatHistory" :key="index" 
                 :class="msg.senderRole !== 'Student' ? 'flex justify-end' : 'flex justify-start'">
              <div 
                   :class="msg.senderRole !== 'Student' ? 'bg-green-100' : 'bg-blue-100'" 
                   class="p-3 rounded-lg max-w-xs">
                
                <p class="text-sm font-semibold text-black">{{ msg.senderName }}</p>
                
                <p class="text-black whitespace-pre-wrap break-all">
                  <template v-for="(part, i) in getMessageParts(msg.message)" :key="i">
                    <a 
                      v-if="part.type === 'url'" 
                      :href="part.content" 
                      target="_blank" 
                      class="text-blue-600 underline hover:text-blue-800"
                    >
                      {{ part.content }}
                    </a>
                    <span v-else>
                      {{ part.content }}
                    </span>
                  </template>
                </p>
                <span class="text-xs text-gray-500">{{ new Date(msg.timestamp).toLocaleString('vi-VN') }}</span>
              </div>
            </div>
            </div>
        </div>

        <div class="bg-gray-50 p-4 rounded">
          <h2 class="text-xl font-semibold mb-4 text-black">Thuộc tính</h2>
          <div class="mb-4">
            <label class="block text-sm font-medium mb-2 text-black">Trạng thái</label>
            <select v-model="selectedStatusId" class="w-full p-2 border rounded text-black">
              <option v-for="status in statuses" :key="status.statusId" :value="status.statusId">
                {{ status.statusName }}
              </option>
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

      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-4 text-black">Hành động</h2>
        <div class="flex gap-4 mb-4">
          <select class="p-2 border rounded text-black">
            <option>Chọn câu trả lời mẫu</option>
            <option>Câu 1: Đã tiếp nhận, đang xử lý.</option>
            <option>Câu 2: Vui lòng cung cấp thêm thông tin.</option>
          </select>
          <button
            class="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700"
          >
            Chèn
          </button>
        </div>
        <form @submit.prevent="sendMessage">
          <textarea
            v-model="newMessage"
            rows="4"
            class="w-full p-3 border rounded text-black placeholder-gray-500"
            placeholder="Nhập tin nhắn..."
          ></textarea>
          <button
            type="submit"
            class="mt-4 bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700"
          >
            Gửi Tin nhắn
          </button>
        </form>
      </div>
    </div>
    
    <div v-else class="text-center text-black">
      Đang tải chi tiết ticket...
    </div>
  </main>
</template>

<script setup lang="ts">
// (GHI CHÚ: Toàn bộ <script> này đã đúng từ câu trước, giữ nguyên)
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { api } from '@/api/axios' 

const route = useRoute()
const ticketId = ref(route.params.id as string)
const ticket = ref<any>(null);
const chatHistory = ref<any[]>([]);
const statuses = ref<any[]>([]);
const selectedStatusId = ref<number | null>(null);
const newMessage = ref('');

function getMessageParts(message: string) {
  if (!message) return [];
  const urlRegex = /(https?:\/\/[^\s]+)/g;
  const parts = message.split(urlRegex);
  return parts.map(part => {
    if (part.match(urlRegex)) {
      return { type: 'url', content: part };
    }
    return { type: 'text', content: part };
  }).filter(part => part.content.length > 0);
}

async function fetchData() {
  try {
    const ticketResponse = await api.get(`/tickets/${ticketId.value}`);
    ticket.value = ticketResponse.data;
    const chatResponse = await api.get(`/tickets/${ticketId.value}/replies`);
    chatHistory.value = chatResponse.data;
    const statusResponse = await api.get(`/statuses`);
    statuses.value = statusResponse.data;
    const currentStatus = statuses.value.find(s => s.statusName === ticket.value.statusName);
    if (currentStatus) {
      selectedStatusId.value = currentStatus.statusId;
    }
  } catch (error) {
    console.error("Lỗi khi tải dữ liệu:", error);
  }
}
onMounted(fetchData);

async function updateStatus() {
  console.log(`Updated status for ticket ${ticketId.value} to ${selectedStatusId.value}`)
  alert("Chức năng đang phát triển!");
}

async function sendMessage() {
  const ktvId = localStorage.getItem('currentUserId'); 
  if (!ktvId || newMessage.value.trim() === '') return;
  const payload = {
    userId: parseInt(ktvId), 
    message: newMessage.value
  };
  try {
    await api.post(`/tickets/${ticketId.value}/replies`, payload);
    newMessage.value = ''; 
    await fetchData(); 
  } catch (error) {
    console.error('Lỗi gửi tin nhắn:', error);
  }
}
</script>

<style scoped>
/* (GHI CHÚ: GIỮ NGUYÊN) */
</style>