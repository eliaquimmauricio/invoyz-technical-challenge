import api from './api'

export const invoiceLineService = {
  getAll() {
    return api.get('/InvoiceLines')
  },
  
  getById(id) {
    return api.get(`/InvoiceLines/${id}`)
  },
  
  getByInvoiceId(invoiceId) {
    return api.get(`/InvoiceLines/invoice/${invoiceId}`)
  },
  
  create(invoiceLine) {
    return api.post('/InvoiceLines', invoiceLine)
  },
  
  update(id, invoiceLine) {
    return api.put(`/InvoiceLines/${id}`, invoiceLine)
  },
  
  delete(id) {
    return api.delete(`/InvoiceLines/${id}`)
  }
}
