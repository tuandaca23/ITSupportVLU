<template>
  <div class="text-black">
    <h1 class="text-3xl font-bold mb-6 text-black">User Management</h1>

    <!-- Search & Filter -->
    <div class="mb-4 flex gap-4 items-center">
      <input
        type="text"
        placeholder="Tìm kiếm theo tên hoặc email"
        class="flex-1 p-2 border rounded text-black placeholder-gray-500"
        v-model="searchQuery"
      />
      <select v-model="roleFilter" class="p-2 border rounded text-black">
        <option value="">Tất cả Role</option>
        <option value="KTV">KTV</option>
        <option value="Admin">Admin</option>
      </select>
    </div>

    <table class="w-full table-auto border-collapse border border-gray-300 text-black">
      <thead>
        <tr class="bg-gray-100 text-black">
          <th class="border border-gray-300 px-4 py-2 text-black">Tên</th>
          <th class="border border-gray-300 px-4 py-2 text-black">Email</th>
          <th class="border border-gray-300 px-4 py-2 text-black">Role</th>
          <th class="border border-gray-300 px-4 py-2 text-black">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="user in filteredUsers"
          :key="user.id"
          class="hover:bg-gray-50 text-black"
        >
          <td class="border border-gray-300 px-4 py-2 text-black">{{ user.name }}</td>
          <td class="border border-gray-300 px-4 py-2 text-black">{{ user.email }}</td>
          <td class="border border-gray-300 px-4 py-2 text-black">{{ user.role }}</td>
          <td class="border border-gray-300 px-4 py-2 text-black flex gap-2 justify-start">
            <button class="bg-yellow-600 text-white px-2 py-1 rounded hover:bg-yellow-700">
              Sửa Role
            </button>
            <button class="bg-red-600 text-white px-2 py-1 rounded hover:bg-red-700">
              Vô hiệu hóa
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'

// Mock data
const users = ref([
  { id: 1, name: 'John Smith', email: 'john@vlu.edu.vn', role: 'KTV' },
  { id: 2, name: 'Emily Davis', email: 'emily@vlu.edu.vn', role: 'Admin' },
  { id: 3, name: 'Nguyen Van A', email: 'a@vlu.edu.vn', role: 'KTV' },
])

const searchQuery = ref('')
const roleFilter = ref('')

// Computed filter
const filteredUsers = computed(() => {
  return users.value.filter(user => {
    const matchesSearch =
      user.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      user.email.toLowerCase().includes(searchQuery.value.toLowerCase())
    const matchesRole = roleFilter.value ? user.role === roleFilter.value : true
    return matchesSearch && matchesRole
  })
})
</script>

<style scoped>
/* Tailwind đã xử lý layout; flex gap giúp actions vừa khít */
</style>
