<template>
  <div class="text-black">
    <h2 class="text-xl font-semibold mb-4">Resolved Tickets (20 gần nhất)</h2>
    <div v-if="loading" class="text-gray-500">Đang tải...</div>
    <div v-if="!loading && tickets.length === 0" class="text-gray-500">Chưa có ticket nào được giải quyết.</div>
    
    <table v-if="!loading && tickets.length > 0" class="w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Mã Ticket</th>
          <th class="border border-gray-300 px-4 py-2">Tiêu đề</th>
          <th class="border border-gray-300 px-4 py-2">Người gửi</th>
          <th class="border border-gray-300 px-4 py-2">Phân loại</th>
          <th class="border border-gray-300 px-4 py-2">Trạng thái</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="ticket in tickets" :key="ticket.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2">{{ ticket.id }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.subject }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.requester }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ ticket.category }}</td>
          <td class="border border-gray-300 px-4 py-2 text-green-700 font-semibold">
            {{ ticket.status }}
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

onMounted(async () => {
  loading.value = true;
  try {
    // Gọi API lấy ticket "Resolved"
    const response = await api.get('/tickets/queue/resolved'); 
    tickets.value = response.data;
  } catch (error) {
    console.error("Lỗi tải ticket đã giải quyết:", error);
  } finally {
    loading.value = false;
  }
});
</script>