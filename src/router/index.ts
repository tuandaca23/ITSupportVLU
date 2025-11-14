import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import StudentDashboard from '../views/StudentDashboard.vue'
import MyRequests from '../views/MyRequests.vue'
import RequestDetail from '../views/RequestDetail.vue'

import KTVDashboard from '../views/KTVDashboard.vue'
import KTVRequestDetail from '../views/KTVRequestDetail.vue'

import AdminDashboard from '../views/AdminDashboard.vue'


const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
    meta: { title: 'IT Support Center' },
  },

  // Routes cho Sinh viên
  { path: '/dashboard-sinhvien', component: StudentDashboard },
  { path: '/my-requests', component: MyRequests },
  { path: '/request-detail/:id', component: RequestDetail, props: true },

  // Routes cho KTV/Admin
  { path: '/dashboard-ktv', component: KTVDashboard },
  { path: '/ktv-request-detail/:id', component: KTVRequestDetail, props: true },

  // Routes cho Admin
  { path: '/dashboard-admin', component: AdminDashboard },
  { path: '/admin-request-detail/:id',
    name: 'AdminRequestDetail',
    component: () => import('../components/admin/AdminRequestDetail.vue'),
    props: true,
  },

]


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
  // (GHI CHÚ: THAY ĐỔI) Nâng cấp hàm scrollBehavior
  scrollBehavior(to, from, savedPosition) {
    // 1. Nếu có vị trí đã lưu (bấm nút Back/Fwd của trình duyệt)
    if (savedPosition) {
      return savedPosition
    }

    // 2. (QUAN TRỌNG) Nếu chỉ thay đổi query (ví dụ: ?page=2)
    //    thì KHÔNG làm gì cả (trả về false/undefined)
    if (to.path === from.path && to.query !== from.query) {
      return; // Giữ nguyên vị trí cuộn
    }

    // 3. Mặc định (chuyển trang mới), cuộn lên đầu
    return { top: 0 }
  },
})

router.afterEach((to) => {
  if (to.meta && typeof to.meta.title === 'string') {
    document.title = to.meta.title
  }
})

export default router
