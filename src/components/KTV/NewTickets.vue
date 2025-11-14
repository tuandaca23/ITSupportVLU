<template>
  <div class="text-black">
    <h2 class="text-xl font-semibold mb-4">Tickets đang chờ KTV (Queue)</h2>
    <div v-if="loading" class="text-gray-500">Đang tải...</div>
    <div v-if="!loading && tickets.length === 0" class="text-gray-500">Không có ticket nào đang chờ.</div>
    
    <table v-if="!loading && tickets.length > 0" class="w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Mã Ticket</th>
          <th class="border border-gray-300 px-4 py-2">Tiêu đề</th>
          <th class="border border-gray-300 px-4 py-2">Người gửi</th>
          <th class="border border-gray-300 px-4 py-2">Phân loại</th>
          <th class="border border-gray-300 px-4 py-2">Trạng thái</th>
          <th class="border border-gray-300 px-4 py-2">Ngày cập nhật</th>
          <th class="border border-gray-300 px-4 py-2">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="ticket in tickets" :key="ticket.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2">{{ ticket.id }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.subject }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.requester }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.category }}</td>
          <td class="border border-gray-300 px-4 py-2 font-semibold text-red-600">{{ ticket.status }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.updatedAt }}</td>
          <td class="border border-gray-300 px-4 py-2">
            <button
              @click="claimTicket(ticket.id)"
              class="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
            >
              Nhận
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { api } from '@/api/axios'

const router = useRouter()
const tickets = ref<any[]>([])
const loading = ref(true)

async function fetchNewTickets() {
  loading.value = true;
  try {
    // Gọi API lấy ticket "Wait"
    const response = await api.get('/tickets/queue/new'); 
    tickets.value = response.data;
  } catch (error) {
    console.error("Lỗi tải ticket chờ:", error);
  } finally {
    loading.value = false;
  }
}
onMounted(fetchNewTickets);

async function claimTicket(id: string) {
  const ktvId = localStorage.getItem('currentUserId');
  if (!ktvId) {
    alert("Không tìm thấy KTV ID");
    return;
  }
  
  try {
    // Gọi API Claim
    await api.post(`/tickets/${id}/claim`, { ktvId: parseInt(ktvId) });
    router.push(`/ktv-request-detail/${id}`);
  } catch (error: any) {
    console.error("Lỗi khi nhận ticket:", error);
    alert("Nhận ticket thất bại: " + (error.response?.data?.message || error.message));
    // Tải lại danh sách, có thể KTV khác đã nhận
    await fetchNewTickets();
  }
}
</script>