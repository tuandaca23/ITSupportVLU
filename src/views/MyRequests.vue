<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-4xl mx-auto bg-white p-8 rounded-lg shadow text-black">
      <h1 class="text-2xl font-bold mb-6 text-black">Yêu cầu của tôi</h1>

      <table class="w-full table-auto text-black">
        <thead>
          <tr class="border-b text-black">
            <th class="text-left p-2 text-black">Mã Ticket</th>
            <th class="text-left p-2 text-black">Tiêu đề</th>
            <th class="text-left p-2 text-black">Phân loại</th>
            <th class="text-left p-2 text-black">Ngày tạo</th>
            <th class="text-left p-2 text-black">Trạng thái</th>
          </tr>
        </thead>
        <tbody>
          <tr class="border-b hover:bg-gray-50 text-black">
            <td class="p-2 text-black">[Placeholder] T001</td>
            <td class="p-2 text-black">
              <router-link to="/request-detail/1" class="text-blue-600 hover:underline">
                [Placeholder] Tiêu đề 1
              </router-link>
            </td>
            <td class="p-2 text-black">[Placeholder] Tài khoản</td>
            <td class="p-2 text-black">[Placeholder] 10-11-2025</td>
            <td class="p-2 text-black">[Placeholder] Mới</td>
          </tr>
          <!-- Thêm hàng placeholder nếu cần -->
        </tbody>
      </table>
    </div>
  </main>
</template>

<script setup lang="ts">
// Không có logic
</script>

<style scoped>
/* Tailwind đã xử lý layout, text-black đảm bảo chữ rõ */
</style>
