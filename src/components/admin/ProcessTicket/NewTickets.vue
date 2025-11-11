<template>
  <div class="mb-8">
    <h2 class="text-xl font-semibold mb-4">📥 New Tickets</h2>
    <div class="overflow-x-auto shadow rounded-lg border border-gray-200 bg-white">
      <table class="w-full table-auto border-collapse text-sm">
        <thead>
          <tr class="bg-gray-100 text-left">
            <th class="px-4 py-2">Mã Ticket</th>
            <th class="px-4 py-2">Tiêu đề</th>
            <th class="px-4 py-2">Người gửi</th>
            <th class="px-4 py-2">Phân loại</th>
            <th class="px-4 py-2">Trạng thái</th>
            <th class="px-4 py-2">Cập nhật</th>
            <th class="px-4 py-2">Gán cho</th>
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
            <td class="px-4 py-2">{{ formatDate(ticket.updatedAt) }}</td>
            <td class="px-4 py-2">
              <div class="flex items-center gap-2">
                <select v-model="ticket.selectedKtv" class="border rounded px-2 py-1 text-sm">
                  <option value="">Chọn KTV</option>
                  <option value="John Smith">John Smith</option>
                  <option value="Emily Davis">Emily Davis</option>
                  <option value="Michael Tran">Michael Tran</option>
                </select>
                <button
                  v-if="!ticket.selectedKtv"
                  @click="processTicket(ticket)"
                  class="bg-blue-600 text-white px-3 py-1.5 rounded hover:bg-blue-700 text-sm"
                >
                  Xử lý
                </button>
                <button
                  v-else
                  @click="assignTicket(ticket)"
                  class="bg-green-600 text-white px-3 py-1.5 rounded hover:bg-green-700 text-sm"
                >
                  Gán
                </button>
              </div>
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
const emit = defineEmits<{ (e: 'process', id: string): void; (e: 'assign', ticket: any): void }>()

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}
function statusColor(status: string) {
  return (
    {
      New: 'bg-yellow-400',
      Assigned: 'bg-gray-400',
      'In Progress': 'bg-blue-500',
      Resolved: 'bg-green-500',
    }[status] || 'bg-gray-300'
  )
}

function processTicket(ticket: any) {
  emit('process', ticket.id)
}
function assignTicket(ticket: any) {
  emit('assign', ticket)
}
</script>
