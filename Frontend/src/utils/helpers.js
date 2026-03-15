// Invoice Status enum matching backend
export const InvoiceStatus = {
  DRAFT: 0,
  PENDING: 1,
  PAID: 2,
  OVERDUE: 3,
  CANCELLED: 4
}

export const InvoiceStatusLabels = {
  [InvoiceStatus.DRAFT]: 'Draft',
  [InvoiceStatus.PENDING]: 'Pending',
  [InvoiceStatus.PAID]: 'Paid',
  [InvoiceStatus.OVERDUE]: 'Overdue',
  [InvoiceStatus.CANCELLED]: 'Cancelled'
}

export const InvoiceStatusColors = {
  [InvoiceStatus.DRAFT]: 'grey',
  [InvoiceStatus.PENDING]: 'warning',
  [InvoiceStatus.PAID]: 'success',
  [InvoiceStatus.OVERDUE]: 'error',
  [InvoiceStatus.CANCELLED]: 'grey-darken-2'
}

// Format currency
export const formatCurrency = (value) => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(value || 0)
}

// Format date
export const formatDate = (date) => {
  if (!date) return ''
  return new Date(date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

// Format date for input fields
export const formatDateForInput = (date) => {
  if (!date) return ''
  const d = new Date(date)
  const year = d.getFullYear()
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${year}-${month}-${day}`
}

// Validate email
export const isValidEmail = (email) => {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return re.test(email)
}
