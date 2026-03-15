<template>
  <v-container fluid class="pa-6">
    <v-row v-if="loading" class="justify-center">
      <v-col cols="12" class="text-center">
        <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      </v-col>
    </v-row>

    <template v-else-if="invoice">
      <v-row>
        <v-col cols="12" class="d-flex justify-space-between align-center">
          <div class="d-flex align-center">
            <v-btn icon @click="goBack" class="mr-4">
              <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <h1 class="text-h3">Invoice {{ invoice.invoiceNumber }}</h1>
          </div>
          <div>
            <v-btn color="primary" class="mr-2" @click="generatePDF">
              <v-icon left>mdi-file-pdf-box</v-icon>
              Download PDF
            </v-btn>
            <v-btn color="secondary" @click="editInvoice">
              <v-icon left>mdi-pencil</v-icon>
              Edit
            </v-btn>
          </div>
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="12" md="8">
          <v-card class="mb-4">
            <v-card-title class="bg-primary">
              <span class="text-white">Invoice Information</span>
            </v-card-title>
            <v-card-text class="pt-4">
              <v-row>
                <v-col cols="12" md="6">
                  <div class="mb-3">
                    <div class="text-caption text-grey">Invoice Number</div>
                    <div class="text-h6">{{ invoice.invoiceNumber }}</div>
                  </div>
                  <div class="mb-3">
                    <div class="text-caption text-grey">Issue Date</div>
                    <div class="text-body-1">{{ formatDate(invoice.issueDate) }}</div>
                  </div>
                  <div class="mb-3">
                    <div class="text-caption text-grey">Due Date</div>
                    <div class="text-body-1">{{ formatDate(invoice.dueDate) }}</div>
                  </div>
                </v-col>
                <v-col cols="12" md="6">
                  <div class="mb-3">
                    <div class="text-caption text-grey">Status</div>
                    <v-chip :color="getStatusColor(invoice.status)" class="mt-1">
                      {{ getStatusLabel(invoice.status) }}
                    </v-chip>
                  </div>
                  <div class="mb-3">
                    <div class="text-caption text-grey">Created</div>
                    <div class="text-body-1">{{ formatDate(invoice.createdAt) }}</div>
                  </div>
                  <div class="mb-3">
                    <div class="text-caption text-grey">Last Updated</div>
                    <div class="text-body-1">{{ formatDate(invoice.updatedAt) }}</div>
                  </div>
                </v-col>
              </v-row>
            </v-card-text>
          </v-card>

          <v-card>
            <v-card-title class="bg-primary">
              <span class="text-white">Invoice Lines</span>
            </v-card-title>
            <v-card-text class="pa-0">
              <v-table>
                <thead>
                  <tr>
                    <th>Product</th>
                    <th class="text-right">Quantity</th>
                    <th class="text-right">Unit Price</th>
                    <th class="text-right">Tax Rate</th>
                    <th class="text-right">Subtotal</th>
                    <th class="text-right">Tax</th>
                    <th class="text-right">Total</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="line in invoice.lines" :key="line.id">
                    <td>
                      <div class="font-weight-medium">{{ line.product?.name || 'Unknown Product' }}</div>
                      <div v-if="line.product?.description" class="text-caption text-grey">
                        {{ line.product.description }}
                      </div>
                    </td>
                    <td class="text-right">{{ line.quantity }}</td>
                    <td class="text-right">{{ formatCurrency(line.unitPrice) }}</td>
                    <td class="text-right">{{ line.taxRate }}%</td>
                    <td class="text-right">{{ formatCurrency(line.lineTotal) }}</td>
                    <td class="text-right">{{ formatCurrency(line.lineTax) }}</td>
                    <td class="text-right font-weight-bold">
                      {{ formatCurrency(line.lineTotal + line.lineTax) }}
                    </td>
                  </tr>
                </tbody>
              </v-table>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col cols="12" md="4">
          <v-card class="mb-4">
            <v-card-title class="bg-primary">
              <span class="text-white">Customer Information</span>
            </v-card-title>
            <v-card-text class="pt-4">
              <div class="mb-3">
                <div class="text-caption text-grey">Name</div>
                <div class="text-h6">{{ invoice.customer?.name }}</div>
              </div>
              <div class="mb-3">
                <div class="text-caption text-grey">VAT Number</div>
                <div class="text-body-1">{{ invoice.customer?.vatNumber }}</div>
              </div>
              <div class="mb-3">
                <div class="text-caption text-grey">Email</div>
                <div class="text-body-1">{{ invoice.customer?.email }}</div>
              </div>
              <div>
                <div class="text-caption text-grey">Address</div>
                <div class="text-body-1">{{ invoice.customer?.address }}</div>
              </div>
            </v-card-text>
          </v-card>

          <v-card>
            <v-card-title class="bg-success">
              <span class="text-white">Invoice Total</span>
            </v-card-title>
            <v-card-text class="pt-4">
              <div class="d-flex justify-space-between mb-2">
                <span class="text-body-1">Subtotal:</span>
                <span class="text-body-1">{{ formatCurrency(invoice.subTotal) }}</span>
              </div>
              <div class="d-flex justify-space-between mb-2">
                <span class="text-body-1">Tax:</span>
                <span class="text-body-1">{{ formatCurrency(invoice.totalTax) }}</span>
              </div>
              <v-divider class="my-3"></v-divider>
              <div class="d-flex justify-space-between">
                <span class="text-h6 font-weight-bold">Grand Total:</span>
                <span class="text-h5 font-weight-bold text-success">
                  {{ formatCurrency(invoice.grandTotal) }}
                </span>
              </div>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </template>

    <v-row v-else>
      <v-col cols="12" class="text-center">
        <v-icon size="64" color="grey">mdi-file-document-alert</v-icon>
        <p class="text-h6 mt-4">Invoice not found</p>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { invoiceService } from '@/services/invoiceService'
import { useNotificationStore } from '@/stores/notification'
import { formatDate, formatCurrency, InvoiceStatusLabels, InvoiceStatusColors } from '@/utils/helpers'

const route = useRoute()
const router = useRouter()
const notificationStore = useNotificationStore()

const invoice = ref(null)
const loading = ref(false)

const getStatusLabel = (status) => InvoiceStatusLabels[status]
const getStatusColor = (status) => InvoiceStatusColors[status]

const loadInvoice = async () => {
  loading.value = true
  try {
    const response = await invoiceService.getById(route.params.id)
    invoice.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading invoice')
  } finally {
    loading.value = false
  }
}

const goBack = () => {
  router.push('/invoices')
}

const editInvoice = () => {
  notificationStore.showInfo('Edit functionality available in the Invoices page')
  router.push('/invoices')
}

const generatePDF = () => {
  // Create a simple HTML representation of the invoice for printing
  const printWindow = window.open('', '_blank')
  if (!printWindow) {
    notificationStore.showError('Please allow popups to generate PDF')
    return
  }

  const htmlContent = `
    <!DOCTYPE html>
    <html>
    <head>
      <title>Invoice ${invoice.value.invoiceNumber}</title>
      <style>
        body {
          font-family: Arial, sans-serif;
          margin: 40px;
          color: #333;
        }
        .header {
          display: flex;
          justify-content: space-between;
          margin-bottom: 40px;
          padding-bottom: 20px;
          border-bottom: 2px solid #1976D2;
        }
        .invoice-title {
          font-size: 32px;
          color: #1976D2;
          margin: 0;
        }
        .invoice-number {
          font-size: 18px;
          color: #666;
        }
        .section {
          margin-bottom: 30px;
        }
        .section-title {
          font-size: 18px;
          font-weight: bold;
          color: #1976D2;
          margin-bottom: 10px;
          padding-bottom: 5px;
          border-bottom: 1px solid #ddd;
        }
        .info-row {
          display: flex;
          margin-bottom: 8px;
        }
        .info-label {
          font-weight: bold;
          width: 150px;
        }
        table {
          width: 100%;
          border-collapse: collapse;
          margin: 20px 0;
        }
        th {
          background-color: #1976D2;
          color: white;
          padding: 12px;
          text-align: left;
        }
        td {
          padding: 10px;
          border-bottom: 1px solid #ddd;
        }
        .text-right {
          text-align: right;
        }
        .totals {
          float: right;
          width: 300px;
          margin-top: 20px;
        }
        .totals-row {
          display: flex;
          justify-content: space-between;
          padding: 8px 0;
        }
        .grand-total {
          font-size: 24px;
          font-weight: bold;
          color: #4CAF50;
          border-top: 2px solid #333;
          padding-top: 15px;
          margin-top: 15px;
        }
        @media print {
          body { margin: 0; }
          .no-print { display: none; }
        }
      </style>
    </head>
    <body>
      <div class="header">
        <div>
          <h1 class="invoice-title">INVOICE</h1>
          <p class="invoice-number">${invoice.value.invoiceNumber}</p>
        </div>
        <div>
          <button class="no-print" onclick="window.print()" style="padding: 10px 20px; background: #1976D2; color: white; border: none; cursor: pointer; border-radius: 4px;">
            Print / Save as PDF
          </button>
        </div>
      </div>

      <div style="display: flex; justify-content: space-between; margin-bottom: 40px;">
        <div class="section" style="flex: 1; margin-right: 20px;">
          <div class="section-title">Customer Information</div>
          <div class="info-row">
            <span class="info-label">Name:</span>
            <span>${invoice.value.customer?.name || 'N/A'}</span>
          </div>
          <div class="info-row">
            <span class="info-label">VAT Number:</span>
            <span>${invoice.value.customer?.vatNumber || 'N/A'}</span>
          </div>
          <div class="info-row">
            <span class="info-label">Email:</span>
            <span>${invoice.value.customer?.email || 'N/A'}</span>
          </div>
          <div class="info-row">
            <span class="info-label">Address:</span>
            <span>${invoice.value.customer?.address || 'N/A'}</span>
          </div>
        </div>

        <div class="section" style="flex: 1;">
          <div class="section-title">Invoice Details</div>
          <div class="info-row">
            <span class="info-label">Issue Date:</span>
            <span>${formatDate(invoice.value.issueDate)}</span>
          </div>
          <div class="info-row">
            <span class="info-label">Due Date:</span>
            <span>${formatDate(invoice.value.dueDate)}</span>
          </div>
          <div class="info-row">
            <span class="info-label">Status:</span>
            <span>${getStatusLabel(invoice.value.status)}</span>
          </div>
        </div>
      </div>

      <div class="section">
        <div class="section-title">Invoice Lines</div>
        <table>
          <thead>
            <tr>
              <th>Product</th>
              <th class="text-right">Qty</th>
              <th class="text-right">Unit Price</th>
              <th class="text-right">Tax Rate</th>
              <th class="text-right">Subtotal</th>
              <th class="text-right">Tax</th>
              <th class="text-right">Total</th>
            </tr>
          </thead>
          <tbody>
            ${invoice.value.lines?.map(line => `
              <tr>
                <td>${line.product?.name || 'Unknown'}</td>
                <td class="text-right">${line.quantity}</td>
                <td class="text-right">${formatCurrency(line.unitPrice)}</td>
                <td class="text-right">${line.taxRate}%</td>
                <td class="text-right">${formatCurrency(line.lineTotal)}</td>
                <td class="text-right">${formatCurrency(line.lineTax)}</td>
                <td class="text-right"><strong>${formatCurrency(line.lineTotal + line.lineTax)}</strong></td>
              </tr>
            `).join('')}
          </tbody>
        </table>
      </div>

      <div class="totals">
        <div class="totals-row">
          <span>Subtotal:</span>
          <span>${formatCurrency(invoice.value.subTotal)}</span>
        </div>
        <div class="totals-row">
          <span>Total Tax:</span>
          <span>${formatCurrency(invoice.value.totalTax)}</span>
        </div>
        <div class="totals-row grand-total">
          <span>Grand Total:</span>
          <span>${formatCurrency(invoice.value.grandTotal)}</span>
        </div>
      </div>
    </body>
    </html>
  `

  printWindow.document.write(htmlContent)
  printWindow.document.close()
  
  notificationStore.showSuccess('PDF preview opened. Use browser print to save as PDF.')
}

onMounted(() => {
  loadInvoice()
})
</script>
