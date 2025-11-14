<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-4xl mx-auto bg-white p-8 rounded-lg shadow text-black" v-if="ticket">
      <h1 class="text-2xl font-bold mb-6 text-black">
        Chi tiết Yêu cầu: {{ ticket.title }}
      </h1>
      <p class="text-black"><strong>Trạng thái:</strong> {{ ticket.statusName }}</p>
      <p class="text-black"><strong>KTV phụ trách:</strong> {{ ticket.assigneeName }}</p>
      <p class="text-black"><strong>Phân loại:</strong> {{ ticket.categoryName }}</p>
      <p class="text-black mt-4"><strong>Mô tả ban đầu:</strong> {{ ticket.description }}</p>

      <div class="my-6 text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Lịch sử trao đổi</h2>
        <div v-if="chatHistory.length === 0" class="text-gray-500">
          Chưa có trao đổi nào.
        </div>
        <div v-else class="space-y-4 max-h-96 overflow-y-auto bg-gray-50 p-4 rounded-lg">
          
          <div v-for="(msg, index) in chatHistory" :key="index" 
               :class="msg.senderRole === 'Student' ? 'flex justify-end' : 'flex justify-start'">
            <div 
                 :class="msg.senderRole === 'Student' ? 'bg-blue-100' : 'bg-green-100'" 
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

      <div v-if="showConnectButtons">
        <p class="text-black font-semibold mb-3">
          Bài viết tự động trên có giải quyết được vấn đề của bạn không?
        </p>
        <div class="flex flex-wrap gap-4 items-center">
          <button
            type="button"
            @click="onMarkAsSolvedClicked"
            class="bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700"
          >
            Đã giải quyết
          </button>
          <button
            type="button"
            @click="onRequestKtvClicked"
            class="bg-red-600 text-white px-6 py-3 rounded hover:bg-red-700"
          >
            Tôi cần kết nối KTV
          </button>
          <button
            type="button"
            @click="goBack"
            class="bg-gray-600 text-white px-6 py-3 rounded hover:bg-gray-700 ml-auto"
          >
            Quay lại Dashboard
          </button>
        </div>
      </div>
      <form @submit.prevent="sendReply" v-if="showChatBox">
        <textarea
          v-model="newMessage"
          placeholder="Gửi phản hồi cho KTV..."
          rows="4"
          class="w-full p-3 border rounded mb-4 text-black placeholder-gray-500"
        ></textarea>
        <div class="flex flex-wrap gap-4">
          <button
            type="submit"
            class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700"
          >
            Gửi
          </button>
          <button
            type="button"
            @click="onMarkAsSolvedClicked"
            class="bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700"
          >
            Đã xong
          </button>
          <button
            type="button"
            @click="goBack"
            class="bg-gray-600 text-white px-6 py-3 rounded hover:bg-gray-700 ml-auto"
          >
            Quay lại
          </button>
        </div>
      </form>
      <div v-if="showGoBackButtonOnly">
         <button
          type="button"
          @click="goBack"
          class="bg-gray-600 text-white px-6 py-3 rounded hover:bg-gray-700"
        >
          Quay lại
        </button>
      </div>
    </div>
    
    <div v-else class="text-center text-black">
      Đang tải chi tiết ticket...
    </div>

    <div
      v-if="showConfirmationModal"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
      @click="closeConfirmationModal"
    >
      <div class="w-full max-w-lg bg-white p-8 rounded-lg shadow text-black" @click.stop>
        <h2 class="text-2xl font-bold mb-4 text-black">{{ confirmationTitle }}</h2>
        <p class="text-black mb-6 whitespace-pre-wrap">{{ confirmationMessage }}</p>
        <div class="flex justify-end gap-4">
          <button
            type="button"
            @click="handleCancel"
            class="bg-gray-200 text-gray-800 px-6 py-3 rounded hover:bg-gray-300"
          >
            Hủy
          </button>
          <button
            type="button"
            @click="handleConfirm"
            class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700"
          >
            OK
          </button>
        </div>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue' 
import { useRouter, useRoute } from 'vue-router'
import { api } from '@/api/axios'

// (GHI CHÚ: GIỮ NGUYÊN) Tất cả các biến ref và computed
const router = useRouter()
const route = useRoute()
const ticketId = route.params.id as string;
const ticket = ref<any>(null); 
const chatHistory = ref<any[]>([]); 
const newMessage = ref(''); 
// (GHI CHÚ: Xóa 'hasRequestedKtv' vì logic computed đã thay đổi)
const showConfirmationModal = ref(false)
const confirmationTitle = ref('')
const confirmationMessage = ref('')
const onConfirmAction = ref<(() => void) | null>(null) 

// (GHI CHÚ: Logic computed đã cập nhật "Wait" và "In Progress")
const showConnectButtons = computed(() => {
  return ticket.value && ticket.value.statusName === 'New';
});
const showChatBox = computed(() => {
  return ticket.value && (ticket.value.statusName === 'In Progress' || ticket.value.statusName === 'Wait');
});
const showGoBackButtonOnly = computed(() => {
  return ticket.value && ticket.value.statusName === 'Resolved';
});

// (GHI CHÚ: GIỮ NGUYÊN) Hàm "linkify"
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

// (GHI CHÚ: GIỮ NGUYÊN) Hàm tải dữ liệu
async function fetchData() {
  try {
    const ticketResponse = await api.get(`/tickets/${ticketId}`);
    ticket.value = ticketResponse.data;
    const chatResponse = await api.get(`/tickets/${ticketId}/replies`);
    chatHistory.value = chatResponse.data;
  } catch (error) {
    console.error("Lỗi khi tải dữ liệu ticket:", error);
  }
}
onMounted(fetchData);

// (GHI CHÚ: GIỮ NGUYÊN) Hàm gửi tin nhắn
async function sendReply() {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId || newMessage.value.trim() === '') return;
  const payload = { userId: parseInt(studentId), message: newMessage.value };
  try {
    await api.post(`/tickets/${ticketId}/replies`, payload);
    newMessage.value = '';
    await fetchData(); 
  } catch (error) {
    console.error('Lỗi gửi tin nhắn:', error);
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) Các hàm Modal (open/close/handle)
function openConfirmation(title: string, message: string, onConfirm: () => void) {
  confirmationTitle.value = title;
  confirmationMessage.value = message;
  onConfirmAction.value = onConfirm; 
  showConfirmationModal.value = true;
}
function closeConfirmationModal() {
  showConfirmationModal.value = false;
  onConfirmAction.value = null; 
}
function handleCancel() {
  closeConfirmationModal();
}
function handleConfirm() {
  if (onConfirmAction.value) {
    onConfirmAction.value(); 
  }
  closeConfirmationModal();
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm API "Mark Solved" (đã dùng DTO)
async function callApiMarkAsSolved() {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId) { /*... (báo lỗi) ...*/ return; }
  
  try {
    await api.post(`/tickets/${ticketId}/mark-solved`, { studentId: parseInt(studentId) });
    router.push('/dashboard-sinhvien'); 
  } catch (error) { /*... (báo lỗi) ...*/ }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm API "Request KTV" (đã dùng DTO)
async function callApiRequestKtv() {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId) { /*... (báo lỗi) ...*/ return; }

  try {
    await api.post(`/tickets/${ticketId}/request-ktv`, { studentId: parseInt(studentId) });
    await fetchData(); // Tải lại (để statusName đổi thành "Wait")
  } catch (error) { /*... (báo lỗi) ...*/ }
}

// (GHI CHÚ: GIỮ NGUYÊN) Các hàm Wrapper (onMark... / onRequest...)
function onMarkAsSolvedClicked() {
  openConfirmation(
    "Xác nhận giải quyết", 
    "Bạn đã chắc chắn vấn đề đã được giải quyết chứ?", 
    callApiMarkAsSolved 
  );
}
function onRequestKtvClicked() {
  openConfirmation(
    "Kết nối KTV",
    "Bạn có muốn gửi yêu cầu kết nối KTV không?\nKTV sẽ hỗ trợ bạn qua khung chat này.",
    callApiRequestKtv 
  );
}
const goBack = () => {
  router.push('/dashboard-sinhvien');
}
</script>

<style scoped>
/* (GHI CHÚ: GIỮ NGUYÊN) */
</style>