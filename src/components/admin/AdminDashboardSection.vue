<template>
  <div class="text-slate-900">
    <h1 class="text-3xl font-bold mb-6">🎫 Dashboard - Quản lý Ticket</h1>

    <div class="flex gap-4 mb-6">
      <button
        v-for="tab in tabs"
        :key="tab.id"
        @click="activeTab = tab.id"
        :class="activeTab === tab.id ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-700'"
        class="px-4 py-2 rounded font-medium"
      >
        {{ tab.label }}
      </button>
    </div>

    <NewTicket
      v-if="activeTab === 'new'"
      :tickets="newTickets"
      @process="goToChatView"
      @assign="assignTicket"
    />
    <InProcessTicket
      v-if="activeTab === 'inprocess'"
      :tickets="inProcessTickets"
      @complete="completeTicket"
      @unassign="unassignTicket"
    />
    <ResolvedTicket
      v-if="activeTab === 'resolved'"
      :tickets="resolvedTickets"
      @restore="restoreTicket"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

import NewTicket from './ProcessTicket/NewTickets.vue'
import InProcessTicket from './ProcessTicket/InProcessTickets.vue'
import ResolvedTicket from './ProcessTicket/ResolvedTickets.vue'

const router = useRouter()

const tabs = [
  { id: 'new', label: 'New Tickets' },
  { id: 'inprocess', label: 'In‑Process Tickets' },
  { id: 'resolved', label: 'Resolved Tickets' },
]

const activeTab = ref('new')

const tickets = ref<any[]>([
  { id: 'T001', subject: 'Lỗi mạng', requester: 'SV A', category: 'Lỗi Mạng', status: 'New', updatedAt: '2023-10-01', selectedKtv: '' },
  { id: 'T002', subject: 'Phần mềm', requester: 'SV B', category: 'Lỗi PM', status: 'New', updatedAt: '2023-10-03', selectedKtv: '' },
  { id: 'T003', subject: 'Máy in', requester: 'GV C', category: 'Thiết bị', status: 'In Progress', updatedAt: '2023-09-29', selectedKtv: 'John Smith' },
  { id: 'T004', subject: 'Đăng nhập', requester: 'SV D', category: 'Lỗi PM', status: 'Resolved', updatedAt: '2023-09-20', selectedKtv: '' },
])

const newTickets = computed(() => tickets.value.filter(t => t.status === 'New'))
const inProcessTickets = computed(() => tickets.value.filter(t => t.status === 'Assigned' || t.status === 'In Progress'))
const resolvedTickets = computed(() => tickets.value.filter(t => t.status === 'Resolved'))

function goToChatView(id: string) {
  router.push(`/admin-request-detail/${id}`)
}

function assignTicket(ticket: any) {
  ticket.status = 'Assigned'
  console.log(`Assigned ticket ${ticket.id} to ${ticket.selectedKtv}`)
}

function completeTicket(ticket: any) {
  ticket.status = 'Resolved'
}

function restoreTicket(ticket: any) {
  ticket.status = 'In Progress'
}

function unassignTicket(ticket: any) {
  ticket.status = 'New'
  ticket.selectedKtv = ''
  console.log(`Unassigned ticket ${ticket.id}, returned to New`)
}
</script>
