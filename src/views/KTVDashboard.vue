<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <header class="mb-8">
      <h1 class="text-3xl font-bold text-slate-900">Chào mừng, Kỹ thuật viên!</h1>
      <p class="mt-2 text-slate-600">Quản lý và xử lý các yêu cầu hỗ trợ IT.</p>
    </header>

    <div class="bg-white p-6 rounded-lg shadow">
      <!-- Tabs -->
      <div class="mb-6">
        <nav class="flex space-x-4">
          <button
            v-for="tab in tabs"
            :key="tab.key"
            @click="activeTab = tab.key"
            :class="activeTab === tab.key ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-700'"
            class="px-4 py-2 rounded"
          >
            {{ tab.label }}
          </button>
        </nav>
      </div>

      <!-- Component hiển thị theo tab -->
      <component :is="currentComponent" />
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'

// Import component từ thư mục con KTV
import NewTickets from '@/components/KTV/NewTickets.vue'
import MyAssignedTickets from '@/components/KTV/MyAssignedTickets.vue'
import ResolvedTickets from '@/components/KTV/ResolvedTickets.vue'

const tabs = [
  { key: 'new', label: 'New Tickets Queue' },
  { key: 'assigned', label: 'My Assigned Tickets' },
  { key: 'resolved', label: 'Resolved Tickets' },
]

const activeTab = ref('new')

const currentComponent = computed(() => {
  if (activeTab.value === 'new') return NewTickets
  if (activeTab.value === 'assigned') return MyAssignedTickets
  if (activeTab.value === 'resolved') return ResolvedTickets
  return null
})
</script>
