import axios from 'axios';

// Tạo một instance (thể hiện) của axios với cấu hình chung
export const api = axios.create({
  baseURL: 'http://localhost:5000/api' // API Backend
});

// (Sau này, khi có login thật, chúng ta sẽ thêm "interceptor"
// vào đây để tự động đính kèm Token vào mọi request)