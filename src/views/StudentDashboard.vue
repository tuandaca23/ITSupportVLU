<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-4xl mx-auto">
      <h1 class="text-3xl font-bold mb-8 text-black">Dashboard Sinh viên</h1>

      <!-- Thanh tìm kiếm -->
      <div class="mb-8">
        <input
          type="text"
          placeholder="Tìm kiếm bài viết (ví dụ: quên mật khẩu)"
          class="w-full p-4 border rounded-lg shadow-sm focus:ring-2 focus:ring-blue-500 text-black placeholder-gray-500"
        />
        <!-- Placeholder cho kết quả tìm kiếm -->
        <div class="mt-4 bg-white p-4 rounded-lg shadow text-black">
          <h3 class="font-semibold mb-2 text-black">Bài viết gợi ý:</h3>
          <ul>
            <li class="mb-1">
              <a href="#" class="text-blue-600 hover:underline">[Placeholder] Bài viết 1</a>
            </li>
            <li class="mb-1">
              <a href="#" class="text-blue-600 hover:underline">[Placeholder] Bài viết 2</a>
            </li>
          </ul>
        </div>
      </div>

      <!-- Nút Tạo Yêu cầu mới -->
      <button
        @click="showModal = true"
        class="inline-block bg-blue-600 text-white px-6 py-3 rounded-lg shadow hover:bg-blue-700 mb-8"
      >
        Tạo Yêu cầu mới
      </button>

      <!-- Danh sách Yêu cầu của tôi -->
      <div class="bg-white p-6 rounded-lg shadow text-black">
        <h2 class="text-xl font-semibold mb-4 text-black">Yêu cầu của tôi (5 gần nhất)</h2>
        <table class="w-full table-auto text-black">
          <thead>
            <tr class="border-b">
              <th class="text-left p-2 text-black">Mã Ticket</th>
              <th class="text-left p-2 text-black">Tiêu đề</th>
              <th class="text-left p-2 text-black">Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            <tr class="border-b hover:bg-gray-50">
              <td class="p-2 text-black">[Placeholder] T001</td>
              <td class="p-2 text-black">
                <router-link to="/request-detail/1" class="text-blue-600 hover:underline">
                  [Placeholder] Tiêu đề 1
                </router-link>
              </td>
              <td class="p-2 text-black">[Placeholder] Mới</td>
            </tr>
            <!-- Thêm các hàng khác nếu cần -->
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal Tạo Yêu cầu mới -->
    <div v-if="showModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50" @click="closeModal">
      <div class="w-full max-w-2xl bg-white p-8 rounded-lg shadow text-black" @click.stop>
        <h2 class="text-2xl font-bold mb-6 text-black">Tạo Yêu cầu mới</h2>
        <form @submit.prevent="submitRequest">
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Tiêu đề</label>
            <input type="text" v-model="form.title" class="w-full p-3 border rounded text-black placeholder-gray-500" placeholder="Nhập tiêu đề" required />
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Phân loại</label>
            <select v-model="form.category" class="w-full p-3 border rounded text-black">
              <option value="">Chọn phân loại vấn đề</option>
              <option>Lỗi Mạng</option>
              <option>Lỗi Phần mềm</option>
              <option>Tài khoản</option>
              <option>Yêu cầu khác</option>
            </select>
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Mô tả</label>
            <textarea v-model="form.description" rows="5" class="w-full p-3 border rounded text-black placeholder-gray-500" placeholder="Mô tả chi tiết vấn đề" required></textarea>
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Đính kèm tệp</label>
            <input type="file" multiple class="w-full text-black" />
          </div>
          <div class="flex space-x-4">
            <button type="submit" class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700">
              Gửi Yêu cầu
            </button>
            <button type="button" @click="closeModal" class="bg-red-600 text-white px-6 py-3 rounded hover:bg-red-700">
              Đóng
            </button>
          </div>
        </form>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const showModal = ref(false)
const form = ref({
  title: '',
  category: '',
  description: ''
})

function closeModal() {
  showModal.value = false
  // Reset form if needed
  form.value = { title: '', category: '', description: '' }
}

function submitRequest() {
  // Mock: Handle form submission (e.g., send to API)
  console.log('Submitted request:', form.value)
  // Close modal after submission
  closeModal()
}
</script>

<style scoped>
/* Tailwind xử lý phần lớn giao diện, chỉ thêm text-black mặc định */
</style>