import api from './api'

export const productService = {
  getAll() {
    return api.get('/Products')
  },
  
  getById(id) {
    return api.get(`/Products/${id}`)
  },
  
  create(product) {
    return api.post('/Products', product)
  },
  
  update(id, product) {
    return api.put(`/Products/${id}`, product)
  },
  
  delete(id) {
    return api.delete(`/Products/${id}`)
  }
}
