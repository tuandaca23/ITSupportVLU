<template>
  <div class="mb-8">
    <h2 class="text-xl font-semibold mb-4">🛠️ In‑Process Tickets</h2>
    <div class="overflow-x-auto shadow rounded-lg border border-gray-200 bg-white">
      <table class="w-full table-auto border-collapse text-sm">
        <thead>
          <tr class="bg-gray-100 text-left">
            <th class="px-4 py-2">Mã Ticket</th>
            <th class="px-4 py-2">Tiêu đề</th>
            <th class="px-4 py-2">Người gửi</th>
            <th class="px-4 py-2">Phân loại</th>
            <th class="px-4 py-2">Trạng thái</th>
            <th class="px-4 py-2">KTV được gán</th>   <!-- Thêm cột này -->
            <th class="px-4 py-2">Cập nhật</th>
            <th class="px-4 py-2">Action</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="ticket in tickets" :key="ticket.id" class="hover:bg-gray-50">
            <td class="px-4 py-2">{{ ticket.id }}</td>
            <td class="px-4 py-2">{{ ticket.subject }}</td>
            <td class="px-4 py-2">{{ ticket.requester }}</td>
            <td class="px-4 py-2">{{ ticket.category }}</td>
            <td class="px-4 py-2 flex items-center gap-2">
              <span class="w-3 h-3 rounded-full" :class="statusColor(ticket.status)"></span>
              {{ ticket.status }}
            </td>
            <td class="px-4 py-2">{{ ticket.selectedKtv || '—' }}</td>   <!-- Hiển thị tên KTV -->
            <td class="px-4 py-2">{{ formatDate(ticket.updatedAt) }}</td>
            <td class="px-4 py-2 flex gap-2">
              <button
                @click="complete(ticket)"
                class="bg-green-600 text-white px-3 py-1.5 rounded hover:bg-green-700 text-sm"
              >
                Hoàn thành
              </button>
              <button
                @click="unassign(ticket)"
                class="bg-yellow-500 text-white px-3 py-1.5 rounded hover:bg-yellow-600 text-sm"
              >
                Bỏ gán
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue'

const props = defineProps<{ tickets: any[] }>()
const emit = defineEmits<{
  (e: 'complete', ticket: any): void
  (e: 'unassign', ticket: any): void
}>()

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

function statusColor(status: string) {
  return {
    'New': 'bg-yellow-400',
    'Assigned': 'bg-gray-400',
    'In Progress': 'bg-blue-500',
    'Resolved': 'bg-green-500',
  }[status] || 'bg-gray-300'
}

function complete(ticket: any) {
  emit('complete', ticket)
}

function unassign(ticket: any) {
  emit('unassign', ticket)
}
</script>
