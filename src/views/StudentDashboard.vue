<template>
  <main class="min-h-screen bg-slate-50 p-8 text-black">
    <div class="max-w-4xl mx-auto">
      <h1 class="text-3xl font-bold mb-8 text-black">Dashboard Sinh viên</h1>

      <div class="mb-8">
        <input
          type="text"
          v-model="searchQuery"
          @input="handleKBSearch" 
          placeholder="Tìm kiếm bài viết (ví dụ: quên mật khẩu)"
          class="w-full p-4 border rounded-lg shadow-sm focus:ring-2 focus:ring-blue-500 text-black placeholder-gray-500"
        />
        <div class="mt-4 bg-white p-4 rounded-lg shadow text-black">
          <h3 class="font-semibold mb-2 text-black">{{ kbTitle }}</h3>
          <div v-if="kbLoading" class="text-gray-500">Đang tìm...</div>
          <div v-if="!kbLoading && kbResults.length === 0" class="text-gray-500">
            Không tìm thấy bài viết nào.
          </div>
          <ul v-if="!kbLoading && kbResults.length > 0">
            <li v-for="article in kbResults" :key="article.articleId" class="mb-1">
              <a :href="article.originalURL" target="_blank" class="text-blue-600 hover:underline">
                {{ article.title }}
              </a>
            </li>
          </ul>
        </div>
      </div>

      <button
        @click="showModal = true"
        class="inline-block bg-blue-600 text-white px-6 py-3 rounded-lg shadow hover:bg-blue-700 mb-8"
      >
        Tạo Yêu cầu mới
      </button>

      <div class="bg-white p-6 rounded-lg shadow text-black" ref="myRequestsList">
        <h2 class="text-xl font-semibold mb-4 text-black">Yêu cầu của tôi</h2>
        
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
          <input
            type="text"
            v-model="ticketSearchQuery"
            @input="resetAndFetchTickets"
            placeholder="Tìm theo tiêu đề ticket..."
            class="w-full p-2 border rounded text-black placeholder-gray-500"
          />
          <select v-model="selectedCategory" @change="resetAndFetchTickets" class="w-full p-2 border rounded text-black">
            <option value="0">Tất cả Phân loại</option>
            <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
              {{ cat.categoryName }}
            </option>
          </select>
          <select v-model="selectedStatus" @change="resetAndFetchTickets" class="w-full p-2 border rounded text-black">
            <option value="0">Tất cả Trạng thái</option>
            <option v-for="status in statuses" :key="status.statusId" :value="status.statusId">
              {{ status.statusName }}
            </option>
          </select>
        </div>

        <div v-if="ticketsLoading" class="text-gray-500 text-center py-4">Đang tải yêu cầu...</div>
        <div v-if="!ticketsLoading && myTickets.length === 0" class="text-gray-500 text-center py-4">
          Không tìm thấy yêu cầu nào.
        </div>

        <table 
          v-if="myTickets.length > 0" 
          class="w-full table-auto text-black transition-opacity"
          :class="{ 'opacity-50': ticketsLoading }"
        >
          <thead>
            <tr class="border-b">
              <th class="text-left p-2 text-black">Tiêu đề</th>
              <th class="text-left p-2 text-black">Phân loại</th>
              <th class="text-left p-2 text-black">Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            <tr 
              v-for="ticket in myTickets" 
              :key="ticket.id" 
              class="border-b hover:bg-gray-100 cursor-pointer"
              @click="goToTicket(ticket.id)"
            >
              <td class="p-2 text-black">{{ ticket.title }}</td>
              <td class="p-2 text-black">{{ ticket.categoryName }}</td>
              <td class="p-2 text-black">{{ ticket.statusName }}</td>
            </tr>
          </tbody>
        </table>

        <div class="mt-4 flex justify-between items-center" v-if="!ticketsLoading && totalPages > 1">
          <button
            @click="changePage(currentPage - 1)"
            :disabled="ticketsLoading || currentPage === 1"
            class="px-4 py-2 bg-gray-200 rounded disabled:opacity-50 disabled:cursor-wait"
          >
            Trước
          </button>
          
          <div class="flex gap-1 items-center">
            <template v-for="(page, index) in visiblePageNumbers" :key="index">
              <button
                v-if="typeof page === 'number'"
                @click="changePage(page)"
                :disabled="ticketsLoading"
                :class="page === currentPage 
                  ? 'bg-blue-600 text-white' 
                  : 'bg-gray-200 text-black hover:bg-gray-300'"
                class="px-4 py-2 rounded disabled:opacity-50 disabled:cursor-wait"
              >
                {{ page }}
              </button>
              <span
                v-else
                class="px-4 py-2 text-gray-500"
              >
                {{ page }}
              </span>
            </template>
          </div>
          
          <button
            @click="changePage(currentPage + 1)"
            :disabled="ticketsLoading || currentPage === totalPages"
            class="px-4 py-2 bg-gray-200 rounded disabled:opacity-50 disabled:cursor-wait"
          >
            Sau
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showModal"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
      @click="closeFormModal"
    >
      <div class="w-full max-w-2xl bg-white p-8 rounded-lg shadow text-black" @click.stop>
        <h2 class="text-2xl font-bold mb-6 text-black">Tạo Yêu cầu mới</h2>
        <form @submit.prevent="submitRequest">
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Tiêu đề</label>
            <input
              type="text"
              v-model="form.title"
              class="w-full p-3 border rounded text-black placeholder-gray-500"
              placeholder="Nhập tiêu đề"
              required
            />
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Phân loại</label>
            <select v-model="form.category" class="w-full p-3 border rounded text-black">
              <option value="0">Chọn phân loại vấn đề</option>
              <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                {{ cat.categoryName }}
              </option>
            </select>
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Mô tả</label>
            <textarea
              v-model="form.description"
              rows="5"
              class="w-full p-3 border rounded text-black placeholder-gray-500"
              placeholder="Mô tả chi tiết vấn đề"
              required
            ></textarea>
          </div>
          <div class="mb-4 text-black">
            <label class="block text-sm font-medium mb-2 text-black">Đính kèm tệp</label>
            <input type="file" multiple class="w-full text-black" />
          </div>
          <div class="flex space-x-4">
            <button
              type="submit"
              class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700"
            >
              Gửi Yêu cầu
            </button>
            <button
              type="button"
              @click="closeFormModal"
              class="bg-red-600 text-white px-6 py-3 rounded hover:bg-red-700"
            >
              Đóng
            </button>
          </div>
        </form>
      </div>
    </div>
    
    <div
      v-if="showNotificationModal"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
      @click="handleNotificationClose"
    >
      <div class="w-full max-w-lg bg-white p-8 rounded-lg shadow text-black" @click.stop>
        <h2 class="text-2xl font-bold mb-4 text-black">{{ notificationTitle }}</h2>
        <p class="text-black mb-6">{{ notificationMessage }}</p>
        <div class="flex justify-end">
          <button
            type="button"
            @click="handleNotificationClose"
            class="bg-blue-600 text-white px-6 py-3 rounded hover:bg-blue-700"
          >
            OK
          </button>
        </div>
      </div>
    </div>
    
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { api } from '@/api/axios' 
import { useRouter } from 'vue-router'

// (GHI CHÚ: GIỮ NGUYÊN) Tất cả các biến ref
const router = useRouter()
const showModal = ref(false)
const form = ref({ title: '', category: 0, description: '' })
const categories = ref<any[]>([]) 
const showNotificationModal = ref(false)
const notificationTitle = ref('')
const notificationMessage = ref('')
const ticketIdToRedirect = ref<number | null>(null)
const searchQuery = ref('') 
const kbResults = ref<any[]>([]) 
const kbLoading = ref(true) 
const kbTitle = ref('Bài viết phổ biến') 
let kbSearchTimeout: any = null; 
const myTickets = ref<any[]>([])
const ticketsLoading = ref(true)
const statuses = ref<any[]>([])
const ticketSearchQuery = ref('') 
const selectedCategory = ref(0) 
const selectedStatus = ref(0) 
const currentPage = ref(1)
const totalPages = ref(1)
let ticketSearchTimeout: any = null; 
const myRequestsList = ref<HTMLElement | null>(null)

// === (GHI CHÚ: ĐÂY LÀ HÀM SỬA LỖI CỦA ANH) ===
// Hàm này gọi API "/my-requests/{studentId}" (API của Sinh viên)
// thay vì API "/queue/new" (API của KTV)
async function fetchMyTickets() {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId) return;
  
  ticketsLoading.value = true;
  try {
    // (GHI CHÚ: Đảm bảo gọi đúng API GetMyRequests)
    const response = await api.get(`/tickets/my-requests/${studentId}`, {
      params: {
        searchTitle: ticketSearchQuery.value,
        categoryId: selectedCategory.value,
        statusId: selectedStatus.value,
        page: currentPage.value
      }
    });
    // (GHI CHÚ: Đảm bảo gán đúng "response.data.tickets")
    myTickets.value = response.data.tickets;
    totalPages.value = response.data.totalPages;
    currentPage.value = response.data.currentPage;
  } catch (error) {
    console.error('Không tải được danh sách ticket:', error);
  } finally {
    ticketsLoading.value = false;
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) Các hàm fetchPopularKB, fetchSearchKB
async function fetchPopularKB() { 
  kbLoading.value = true;
  kbTitle.value = 'Bài viết phổ biến';
  try {
    const response = await api.get('/kb/popular');
    kbResults.value = response.data;
  } catch (error) { console.error('Không tải được KB phổ biến:', error);
  } finally { kbLoading.value = false; }
}
async function fetchSearchKB() { 
  kbLoading.value = true;
  if (searchQuery.value.trim() === '') {
    fetchPopularKB();
    return;
  }
  kbTitle.value = `Kết quả cho "${searchQuery.value}"`;
  try {
    const response = await api.get(`/kb/search?query=${searchQuery.value}`);
    kbResults.value = response.data;
  } catch (error) { console.error('Lỗi khi tìm kiếm KB:', error);
  } finally { kbLoading.value = false; }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm handleKBSearch
function handleKBSearch() {
  clearTimeout(kbSearchTimeout); 
  kbSearchTimeout = setTimeout(() => { fetchSearchKB(); }, 500); 
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm resetAndFetchTickets (đã tối ưu)
function resetAndFetchTickets() {
  clearTimeout(ticketSearchTimeout); 
  ticketSearchTimeout = setTimeout(async () => {
    currentPage.value = 1; 
    myRequestsList.value?.scrollIntoView({ behavior: 'smooth' });
    await fetchMyTickets(); 
  }, 500); 
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm changePage (đã sửa lỗi type)
async function changePage(page: number | string) { 
  if (typeof page === 'string') {
    return; 
  }
  if (page < 1 || page > totalPages.value) return;
  myRequestsList.value?.scrollIntoView({ behavior: 'smooth' });
  currentPage.value = page;
  await fetchMyTickets();
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm visiblePageNumbers (đã sửa lỗi type)
const visiblePageNumbers = computed(() => {
  const total = totalPages.value;
  const current = currentPage.value;
  const maxPages = 7; 
  if (total <= maxPages) {
    return Array.from({ length: total }, (_, i) => i + 1);
  }
  const pages: (number | string)[] = []; 
  pages.push(1);
  let start = Math.max(2, current - 2);
  let end = Math.min(total - 1, current + 2);
  if (current <= 4) { end = 5; }
  if (current >= total - 3) { start = total - 4; }
  if (start > 2) {
    pages.push('...'); 
  }
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }
  if (end < total - 1) {
    pages.push('...');
  }
  pages.push(total);
  return pages;
});

// (GHI CHÚ: GIỮ NGUYÊN) Hàm fetchStatuses (đã gọi API)
async function fetchStatuses() {
  try {
    const response = await api.get('/statuses'); 
    statuses.value = response.data;
  } catch (error) {
    console.error('Không tải được danh sách trạng thái:', error);
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) onMounted (đã gọi fetchStatuses)
onMounted(async () => {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId) {
    router.push('/');
    return;
  }
  
  await Promise.all([
    fetchCategories(),
    fetchStatuses(), 
    fetchMyTickets(),
    fetchPopularKB() 
  ]);
});

// (GHI CHÚ: GIỮ NGUYÊN) Hàm fetchCategories
async function fetchCategories() {
  try {
    const response = await api.get('/categories')
    categories.value = response.data
  } catch (error) {
    console.error('Không tải được danh mục:', error)
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm submitRequest
async function submitRequest() {
  const studentId = localStorage.getItem('currentUserId');
  if (!studentId || form.value.category === 0) { 
    notificationTitle.value = 'Lỗi'
    notificationMessage.value = 'Vui lòng điền đầy đủ thông tin và chọn phân loại.'
    showNotificationModal.value = true
    return; 
  }
  const payload = {
    studentId: parseInt(studentId),
    title: form.value.title,
    description: form.value.description,
    categoryId: form.value.category,
  };
  try {
    const response = await api.post('/tickets', payload)
    const newTicketId = response.data.ticketId
    closeFormModal()
    currentPage.value = 1; 
    await fetchMyTickets(); // Tải lại danh sách ticket
    notificationTitle.value = 'Thành công!'
    notificationMessage.value = 'Gửi yêu cầu thành công...'
    ticketIdToRedirect.value = newTicketId
    showNotificationModal.value = true
  } catch (error) {
    console.error('Lỗi khi gửi ticket:', error)
    notificationTitle.value = 'Gửi thất bại'
    notificationMessage.value = 'Đã có lỗi xảy ra. Vui lòng thử lại.'
    ticketIdToRedirect.value = null
    showNotificationModal.value = true
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm closeFormModal
function closeFormModal() {
  showModal.value = false
  form.value = { title: '', category: 0, description: '' }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm handleNotificationClose
function handleNotificationClose() {
  showNotificationModal.value = false
  if (ticketIdToRedirect.value) {
    router.push(`/request-detail/${ticketIdToRedirect.value}`)
    ticketIdToRedirect.value = null
  }
}

// (GHI CHÚ: GIỮ NGUYÊN) Hàm goToTicket
function goToTicket(ticketId: number) {
  router.push(`/request-detail/${ticketId}`);
}
</script>

<style scoped>
/* (GHI CHÚ: GIỮ NGUYÊN) */
/* Tailwind xử lý phần lớn giao diện */
</style>