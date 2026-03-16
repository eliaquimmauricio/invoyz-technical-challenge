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
          </div>
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="12" md="8">
          <v-card class="mb-4">
            <v-card-title class="card-header">
              <v-icon left color="primary">mdi-file-document-outline</v-icon>
              <span>Invoice Information</span>
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
            <v-card-title class="card-header">
              <v-icon left color="primary">mdi-format-list-bulleted</v-icon>
              <span>Invoice Lines</span>
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
            <v-card-title class="card-header">
              <v-icon left color="primary">mdi-account</v-icon>
              <span>Customer Information</span>
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

          <v-card class="total-card">
            <v-card-title class="card-header">
              <v-icon left color="success">mdi-calculator</v-icon>
              <span>Invoice Total</span>
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

const generatePDF = async () => {
  await invoiceService.generatePDF(route.params.id)
    .then(() => notificationStore.showSuccess('The PDF will be generated shortly. Please check your email.'))
    .catch(() => notificationStore.showError('Error generating PDF'))
}

onMounted(() => {
  loadInvoice()
})
</script>

<style scoped>
.card-header {
  background: linear-gradient(135deg, rgba(99, 102, 241, 0.1) 0%, rgba(139, 92, 246, 0.1) 100%);
  border-bottom: 2px solid rgba(99, 102, 241, 0.2);
  font-weight: 600;
  display: flex;
  align-items: center;
}

.total-card {
  border: 2px solid rgba(16, 185, 129, 0.3);
}

.text-success {
  color: #10B981 !important;
}
</style>
