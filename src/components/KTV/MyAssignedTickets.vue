<template>
  <div class="text-black">
    <h2 class="text-xl font-semibold mb-4">Tickets của tôi (Đang xử lý)</h2>
    <div v-if="loading" class="text-gray-500">Đang tải...</div>
    <div v-if="!loading && tickets.length === 0" class="text-gray-500">Bạn không có ticket nào đang xử lý.</div>
    
    <table v-if="!loading && tickets.length > 0" class="w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Mã Ticket</th>
          <th class="border border-gray-300 px-4 py-2">Tiêu đề</th>
          <th class="border border-gray-300 px-4 py-2">Người gửi</th>
          <th class="border border-gray-300 px-4 py-2">Phân loại</th>
          <th class="border border-gray-300 px-4 py-2">Trạng thái</th>
          <th class="border border-gray-300 px-4 py-2">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="ticket in tickets" :key="ticket.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2">{{ ticket.id }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.subject }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.requester }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.category }}</td>
          <td class="border border-gray-300 px-4 py-2 font-semibold text-blue-600">{{ ticket.status }}</td>
          <td class="border border-gray-300 px-4 py-2">
            <router-link
              :to="`/ktv-request-detail/${ticket.id}`"
              class="bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700"
            >
              Xem
            </router-link>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { api } from '@/api/axios'

const tickets = ref<any[]>([])
const loading = ref(true)
const ktvId = localStorage.getItem('currentUserId');

onMounted(async () => {
  if (!ktvId) {
    loading.value = false;
    return;
  }
  loading.value = true;
  try {
    // Gọi API lấy ticket "In Progress" của KTV này
    const response = await api.get(`/tickets/queue/assigned/${ktvId}`); 
    tickets.value = response.data;
  } catch (error) {
    console.error("Lỗi tải ticket của tôi:", error);
  } finally {
    loading.value = false;
  }
});
</script>