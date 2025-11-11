<template>
  <div class="flex min-h-screen bg-slate-50">
    <!-- Sidebar -->
    <aside class="w-64 bg-white shadow-lg">
      <div class="p-6">
        <h2 class="text-xl font-bold text-slate-900">Admin Panel</h2>
      </div>
      <nav class="mt-6">
        <button
          @click="activeSection = 'dashboard'"
          :class="activeSection === 'dashboard' ? 'bg-blue-100 text-blue-700' : 'text-gray-700'"
          class="w-full text-left px-6 py-3 hover:bg-gray-100"
        >
          Dashboard
        </button>
        <button
          @click="activeSection = 'user-management'"
          :class="
            activeSection === 'user-management' ? 'bg-blue-100 text-blue-700' : 'text-gray-700'
          "
          class="w-full text-left px-6 py-3 hover:bg-gray-100"
        >
          Quản lý Người dùng
        </button>
        <button
          @click="activeSection = 'kb-management'"
          :class="activeSection === 'kb-management' ? 'bg-blue-100 text-blue-700' : 'text-gray-700'"
          class="w-full text-left px-6 py-3 hover:bg-gray-100"
        >
          Quản lý KB
        </button>
        <button
          @click="activeSection = 'reports'"
          :class="activeSection === 'reports' ? 'bg-blue-100 text-blue-700' : 'text-gray-700'"
          class="w-full text-left px-6 py-3 hover:bg-gray-100"
        >
          Reports
        </button>
      </nav>
    </aside>

    <!-- Main Content -->
    <main class="flex-1 p-8">
      <AdminDashboardSection v-if="activeSection === 'dashboard'" />
      <AdminReportsSection v-if="activeSection === 'reports'" />
      <AdminUserManagementSection v-if="activeSection === 'user-management'" />
      <AdminKBManagementSection v-if="activeSection === 'kb-management'" />
      <AdminRequestDetail v-if="activeSection === 'request-detail'" />
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AdminDashboardSection from '@/components/admin/AdminDashboardSection.vue'
import AdminReportsSection from '@/components/admin/AdminReportsSection.vue'
import AdminUserManagementSection from '@/components/admin/AdminUserManagementSection.vue'
import AdminKBManagementSection from '@/components/admin/AdminKBManagementSection.vue'
import AdminRequestDetail from '@/components/admin/AdminRequestDetail.vue'

const activeSection = ref('dashboard')
</script>

<style scoped>
/* Tailwind handles styling */
</style>
