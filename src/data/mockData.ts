export interface Ticket {
  id: string
  title: string
  category: string
  description: string
  status: 'Mới' | 'Đang xử lý' | 'Chờ phản hồi SV' | 'Đã giải quyết' | 'Đã đóng'
  createdAt: string
  studentId: string
  assignedTo?: string
  replies: { from: string; message: string; timestamp: string; isInternal?: boolean }[]
  attachments?: string[]
}

export const mockTickets: Ticket[] = [
  {
    id: 'T001',
    title: 'Quên mật khẩu email',
    category: 'Tài khoản',
    description: 'Tôi quên mật khẩu email trường.',
    status: 'Đang xử lý',
    createdAt: '2023-10-01',
    studentId: 'SV001',
    assignedTo: 'KTV001',
    replies: [
      { from: 'KTV001', message: 'Vui lòng cung cấp mã sinh viên.', timestamp: '2023-10-02' },
      { from: 'SV001', message: 'Mã của tôi là 12345.', timestamp: '2023-10-03' },
    ],
  },
  // Thêm nhiều ticket khác...
]

export const categories = ['Lỗi Mạng', 'Lỗi Phần mềm', 'Tài khoản', 'Yêu cầu khác']
export const statuses = ['Mới', 'Đang xử lý', 'Chờ phản hồi SV', 'Đã giải quyết', 'Đã đóng']