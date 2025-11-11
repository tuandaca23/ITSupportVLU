<template>
  <main class="min-h-screen bg-gradient-to-br from-slate-50 via-slate-100 to-slate-200">
    <div class="mx-auto flex min-h-screen max-w-7xl items-center justify-center px-4 py-12 sm:px-6 lg:px-8">
      <section
        class="w-full max-w-xl rounded-2xl bg-white/90 p-8 shadow-2xl ring-1 ring-black/5 backdrop-blur-sm sm:p-10"
        aria-labelledby="login-heading"
      >
        <div class="flex w-full flex-col items-center text-center">
          <div class="mb-6 flex h-14 w-14 items-center justify-center rounded-full bg-gradient-to-br from-blue-500 to-blue-600 text-white shadow-lg shadow-blue-500/20">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-7 w-7">
              <path
                fill-rule="evenodd"
                d="M11.7 2.1a.75.75 0 0 1 .6 0l7.5 3.214a.75.75 0 0 1 .45.69v6.91c0 2.218-1.29 4.27-3.302 5.2l-4.3 1.992a.75.75 0 0 1-.596 0l-4.3-1.992A5.85 5.85 0 0 1 3.75 12.914V6.004a.75.75 0 0 1 .45-.69L11.7 2.1Zm3.78 7.72a.75.75 0 0 0-1.06-1.06l-3.97 3.97-1.57-1.57a.75.75 0 0 1 0-1.06l2.1 2.1c.293.293.768.293 1.06 0l4.5-4.5Z"
                clip-rule="evenodd"
              />
            </svg>
          </div>

          <h1 id="login-heading" class="text-2xl font-semibold text-slate-900">IT Support Center VLU</h1>
          <p class="mt-1 text-slate-600">Đăng nhập để truy cập các dịch vụ hỗ trợ</p>

          <button
            type="button"
            class="group mt-8 inline-flex w-full items-center justify-center gap-3 rounded-xl bg-gradient-to-r from-blue-600 to-blue-500 px-6 py-4 text-base font-medium text-white shadow-lg shadow-blue-500/30 transition hover:from-blue-700 hover:to-blue-600 focus:outline-none focus:ring-4 focus:ring-blue-300"
            @click="onLogin"
          >
            <span class="flex h-6 w-6 items-center justify-center rounded-md bg-white/15">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-5 w-5">
                <path
                  fill-rule="evenodd"
                  d="M3.75 12a.75.75 0 0 1 .75-.75h8.69l-2.22-2.22a.75.75 0 0 1 1.06-1.06l3.5 3.5a.75.75 0 0 1 0 1.06l-3.5 3.5a.75.75 0 0 1-1.06-1.06l2.22-2.22H4.5A.75.75 0 0 1 3.75 12Zm12-6a.75.75 0 0 1 .75-.75h3a.75.75 0 0 1 .75.75v12a.75.75 0 0 1-.75.75h-3a.75.75 0 0 1-.75-.75V6Z"
                  clip-rule="evenodd"
                />
              </svg>
            </span>
            <span>Đăng nhập bằng tài khoản Trường</span>
          </button>

          <p class="mt-3 text-sm text-slate-500">Sử dụng tài khoản do trường cấp để truy cập hệ thống IT Support</p>
        </div>
      </section>
    </div>

    <!-- Modal chọn vai trò -->
    <div v-if="showModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50" @click="closeModal">
      <div class="w-full max-w-md rounded-2xl bg-white p-8 shadow-2xl ring-1 ring-black/5" @click.stop>
        <h2 class="text-xl font-semibold text-slate-900 text-center mb-6">Chọn vai trò của bạn</h2>
        <div class="flex flex-col gap-4">
          <button
            @click="selectRole('sinhvien')"
            class="inline-flex w-full items-center justify-center gap-3 rounded-xl bg-gradient-to-r from-green-600 to-green-500 px-6 py-4 text-base font-medium text-white shadow-lg shadow-green-500/30 transition hover:from-green-700 hover:to-green-600 focus:outline-none focus:ring-4 focus:ring-green-300"
          >
            <span class="flex h-6 w-6 items-center justify-center rounded-md bg-white/15">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-5 w-5">
                <path d="M4.26 10.147a60.438 60.438 0 0 0-.491 6.347A48.62 48.62 0 0 1 12 20.904a48.62 48.62 0 0 1 8.232-4.41 60.46 60.46 0 0 0-.491-6.347m-15.482 0a50.636 50.636 0 0 0-2.658-.813A59.906 59.906 0 0 1 12 3.493a59.903 59.903 0 0 1 10.399 5.84c-.896.248-1.783.52-2.658.814m-15.482 0A50.717 50.717 0 0 1 12 13.489a50.702 50.702 0 0 1 7.74-3.342M6.75 15a.75.75 0 0 0 0-1.5.75.75 0 0 0 0 1.5Zm0 0v-3.675A55.378 55.378 0 0 1 12 8.443a55.381 55.381 0 0 1 5.25 2.882V15" />
              </svg>
            </span>
            <span>Sinh viên</span>
          </button>
          <button
            @click="selectRole('ktv')"
            class="inline-flex w-full items-center justify-center gap-3 rounded-xl bg-gradient-to-r from-purple-600 to-purple-500 px-6 py-4 text-base font-medium text-white shadow-lg shadow-purple-500/30 transition hover:from-purple-700 hover:to-purple-600 focus:outline-none focus:ring-4 focus:ring-purple-300"
          >
            <span class="flex h-6 w-6 items-center justify-center rounded-md bg-white/15">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-5 w-5">
                <path fill-rule="evenodd" d="M7.5 6a4.5 4.5 0 1 1 9 0 4.5 4.5 0 0 1-9 0ZM3.751 20.105a8.25 8.25 0 0 1 16.498 0 .75.75 0 0 1-.437.695A18.683 18.683 0 0 1 12 22.5c-2.786 0-5.433-.608-7.812-1.7a.75.75 0 0 1-.437-.695Z" clip-rule="evenodd" />
              </svg>
            </span>
            <span>KTV</span>
          </button>
          <button
            @click="selectRole('admin')"
            class="inline-flex w-full items-center justify-center gap-3 rounded-xl bg-gradient-to-r from-red-600 to-red-500 px-6 py-4 text-base font-medium text-white shadow-lg shadow-red-500/30 transition hover:from-red-700 hover:to-red-600 focus:outline-none focus:ring-4 focus:ring-red-300"
          >
            <span class="flex h-6 w-6 items-center justify-center rounded-md bg-white/15">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-5 w-5">
                <path fill-rule="evenodd" d="M12 1.5a5.25 5.25 0 0 0-5.25 5.25v3a3 3 0 0 0-3 3v6.75a3 3 0 0 0 3 3h10.5a3 3 0 0 0 3-3v-6.75a3 3 0 0 0-3-3v-3c0-2.9-2.35-5.25-5.25-5.25Zm3.75 8.25v-3a3.75 3.75 0 1 0-7.5 0v3h7.5Z" clip-rule="evenodd" />
              </svg>
            </span>
            <span>Admin</span>
          </button>
          <button
            @click="closeModal"
            class="mt-4 inline-flex w-full items-center justify-center rounded-xl bg-slate-200 px-6 py-3 text-base font-medium text-slate-700 transition hover:bg-slate-300 focus:outline-none focus:ring-4 focus:ring-slate-300"
          >
            Hủy
          </button>
        </div>
      </div>
    </div>

    <a
      class="fixed bottom-4 right-4 inline-flex h-10 w-10 items-center justify-center rounded-full bg-slate-900 text-white shadow-lg ring-1 ring-black/5 transition hover:bg-slate-800"
      href="mailto:pcntt@vlu.edu.vn"
      aria-label="Liên hệ hỗ trợ"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="h-5 w-5">
        <path
          fill-rule="evenodd"
          d="M12 2.25a9.75 9.75 0 1 0 0 19.5 9.75 9.75 0 0 0 0-19.5ZM9.75 9a2.25 2.25 0 1 1 3.487 1.88c-.233.145-.487.37-.654.672-.147.27-.229.58-.229.948a.75.75 0 0 1-1.5 0c0-.62.145-1.175.39-1.615.244-.438.57-.75.9-.957A.75.75 0 0 0 12 9.75a.75.75 0 0 0 0-1.5.75.75 0 0 0-.75.75  .75.75 0 0 1-1.5 0Zm2.25 6a.75.75 0 0 0-1.5 0v.008a.75.75 0 0 0 1.5 0V15Z"
          clip-rule="evenodd"
        />
      </svg>
    </a>

    <p class="pointer-events-none fixed bottom-4 left-0 right-0 mx-auto w-full max-w-3xl px-6 text-center text-sm text-slate-500">
      Cần trợ giúp? Liên hệ IT: <a class="underline decoration-slate-300 underline-offset-2 hover:text-slate-700" href="mailto:pcntt@vlu.edu.vn">pcntt@vlu.edu.vn</a> · <a class="underline decoration-slate-300 underline-offset-2 hover:text-slate-700" href="mailto:hotro@vlu.edu.vn">hotro@vlu.edu.vn</a>
    </p>
  </main>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const showModal = ref(false)

function onLogin() {
  showModal.value = true
}

function selectRole(role: string) {
  if (role === 'sinhvien') {
    // Thay đổi URL này thành dashboard thực tế cho sinh viên
    window.location.href = '/dashboard-sinhvien'
  } else if (role === 'ktv') {
    // Thay đổi URL này thành dashboard thực tế cho KTV
    window.location.href = '/dashboard-ktv'
  } else if (role === 'admin') {
    // Thay đổi URL này thành dashboard thực tế cho Admin
    window.location.href = '/dashboard-admin'
  }
  showModal.value = false
}

function closeModal() {
  showModal.value = false
}
</script>

<style scoped>
</style>