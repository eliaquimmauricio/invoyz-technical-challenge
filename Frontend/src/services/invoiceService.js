import api from './api'

export const invoiceService = {
  getAll() {
    return api.get('/Invoices')
  },
  
  getById(id) {
    return api.get(`/Invoices/${id}`)
  },
  
  create(invoice) {
    return api.post('/Invoices', invoice)
  },
  
  update(id, invoice) {
    return api.put(`/Invoices/${id}`, invoice)
  },
  
  delete(id) {
    return api.delete(`/Invoices/${id}`)
  },
  
  generatePDF(id) {
    return api.post(`/Invoices/${id}/generate-pdf`, {
      responseType: 'blob'
    })
  }
}
