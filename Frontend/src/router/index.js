import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: () => import('@/views/Dashboard.vue')
  },
  {
    path: '/customers',
    name: 'Customers',
    component: () => import('@/views/Customers.vue')
  },
  {
    path: '/products',
    name: 'Products',
    component: () => import('@/views/Products.vue')
  },
  {
    path: '/invoices',
    name: 'Invoices',
    component: () => import('@/views/Invoices.vue')
  },
  {
    path: '/invoices/:id',
    name: 'InvoiceDetail',
    component: () => import('@/views/InvoiceDetail.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
