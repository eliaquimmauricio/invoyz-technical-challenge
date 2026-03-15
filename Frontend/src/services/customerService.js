import api from './api'

export const customerService = {
  getAll() {
    return api.get('/Customers')
  },
  
  getById(id) {
    return api.get(`/Customers/${id}`)
  },
  
  create(customer) {
    return api.post('/Customers', customer)
  },
  
  update(id, customer) {
    return api.put(`/Customers/${id}`, customer)
  },
  
  delete(id) {
    return api.delete(`/Customers/${id}`)
  }
}
