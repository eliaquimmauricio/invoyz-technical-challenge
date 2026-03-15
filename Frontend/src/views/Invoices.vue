<template>
  <v-container fluid class="pa-6">
    <v-row>
      <v-col cols="12" class="d-flex justify-space-between align-center">
        <h1 class="text-h3">Invoices</h1>
        <v-btn color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Add Invoice
        </v-btn>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>
            <v-text-field
              v-model="search"
              append-inner-icon="mdi-magnify"
              label="Search"
              single-line
              hide-details
              variant="outlined"
              density="compact"
            ></v-text-field>
          </v-card-title>

          <v-data-table
            :headers="headers"
            :items="invoices"
            :search="search"
            :loading="loading"
            class="elevation-1"
          >
            <template v-slot:item.customer.name="{ item }">
              {{ item.customer?.name || 'N/A' }}
            </template>

            <template v-slot:item.issueDate="{ item }">
              {{ formatDate(item.issueDate) }}
            </template>

            <template v-slot:item.dueDate="{ item }">
              {{ formatDate(item.dueDate) }}
            </template>

            <template v-slot:item.status="{ item }">
              <v-chip :color="getStatusColor(item.status)" size="small">
                {{ getStatusLabel(item.status) }}
              </v-chip>
            </template>

            <template v-slot:item.grandTotal="{ item }">
              {{ formatCurrency(item.grandTotal) }}
            </template>

            <template v-slot:item.actions="{ item }">
              <v-icon
                size="small"
                class="mr-2"
                @click="viewInvoice(item)"
              >
                mdi-eye
              </v-icon>
              <v-icon
                size="small"
                class="mr-2"
                @click="openDialog(item)"
              >
                mdi-pencil
              </v-icon>
              <v-icon
                size="small"
                @click="deleteInvoice(item)"
              >
                mdi-delete
              </v-icon>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- Invoice Dialog -->
    <v-dialog v-model="dialog" max-width="900px" persistent scrollable>
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ editedItem.id ? 'Edit Invoice' : 'New Invoice' }}</span>
        </v-card-title>

        <v-card-text style="max-height: 600px;">
          <v-form ref="form" v-model="valid">
            <v-row>
              <v-col cols="12" md="6">
                <v-text-field
                  v-model="editedItem.invoiceNumber"
                  label="Invoice Number*"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12" md="6">
                <v-select
                  v-model="editedItem.customerId"
                  :items="customers"
                  item-title="name"
                  item-value="id"
                  label="Customer*"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-select>
              </v-col>

              <v-col cols="12" md="4">
                <v-text-field
                  v-model="editedItem.issueDate"
                  label="Issue Date*"
                  type="date"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12" md="4">
                <v-text-field
                  v-model="editedItem.dueDate"
                  label="Due Date*"
                  type="date"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12" md="4">
                <v-select
                  v-model="editedItem.status"
                  :items="statusOptions"
                  label="Status*"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-select>
              </v-col>

              <!-- Invoice Lines -->
              <v-col cols="12">
                <v-divider class="my-4"></v-divider>
                <div class="d-flex justify-space-between align-center mb-4">
                  <h3 class="text-h6">Invoice Lines</h3>
                  <v-btn color="primary" size="small" @click="addLine">
                    <v-icon left size="small">mdi-plus</v-icon>
                    Add Line
                  </v-btn>
                </div>

                <v-card
                  v-for="(line, index) in editedItem.lines"
                  :key="index"
                  class="mb-3"
                  variant="outlined"
                >
                  <v-card-text>
                    <v-row>
                      <v-col cols="12" md="5">
                        <v-select
                          v-model="line.productId"
                          :items="products"
                          item-title="name"
                          item-value="id"
                          label="Product*"
                          variant="outlined"
                          density="compact"
                          @update:model-value="updateLineFromProduct(line)"
                        ></v-select>
                      </v-col>

                      <v-col cols="6" md="2">
                        <v-text-field
                          v-model.number="line.quantity"
                          label="Quantity*"
                          type="number"
                          variant="outlined"
                          density="compact"
                          min="1"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="6" md="2">
                        <v-text-field
                          v-model.number="line.unitPrice"
                          label="Unit Price*"
                          type="number"
                          variant="outlined"
                          density="compact"
                          prefix="$"
                          step="0.01"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="6" md="2">
                        <v-text-field
                          v-model.number="line.taxRate"
                          label="Tax Rate (%)*"
                          type="number"
                          variant="outlined"
                          density="compact"
                          suffix="%"
                          step="0.01"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="6" md="1" class="d-flex align-center">
                        <v-btn
                          icon
                          size="small"
                          color="error"
                          @click="removeLine(index)"
                        >
                          <v-icon size="small">mdi-delete</v-icon>
                        </v-btn>
                      </v-col>
                    </v-row>
                  </v-card-text>
                </v-card>

                <v-alert v-if="editedItem.lines.length === 0" type="info" variant="outlined">
                  No invoice lines added yet. Click "Add Line" to get started.
                </v-alert>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="closeDialog">Cancel</v-btn>
          <v-btn color="primary" variant="flat" @click="saveInvoice" :disabled="!valid || editedItem.lines.length === 0">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card>
        <v-card-title class="text-h5">Confirm Delete</v-card-title>
        <v-card-text>Are you sure you want to delete this invoice?</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="deleteDialog = false">Cancel</v-btn>
          <v-btn color="error" variant="flat" @click="confirmDelete">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { invoiceService } from '@/services/invoiceService'
import { customerService } from '@/services/customerService'
import { productService } from '@/services/productService'
import { useNotificationStore } from '@/stores/notification'
import { formatDate, formatCurrency, InvoiceStatus, InvoiceStatusLabels, InvoiceStatusColors, formatDateForInput } from '@/utils/helpers'

const router = useRouter()
const notificationStore = useNotificationStore()

const invoices = ref([])
const customers = ref([])
const products = ref([])
const loading = ref(false)
const search = ref('')
const dialog = ref(false)
const deleteDialog = ref(false)
const valid = ref(false)
const form = ref(null)

const headers = [
  { title: 'Invoice #', key: 'invoiceNumber', sortable: true },
  { title: 'Customer', key: 'customer.name', sortable: true },
  { title: 'Issue Date', key: 'issueDate', sortable: true },
  { title: 'Due Date', key: 'dueDate', sortable: true },
  { title: 'Status', key: 'status', sortable: true },
  { title: 'Total', key: 'grandTotal', sortable: true },
  { title: 'Actions', key: 'actions', sortable: false, align: 'end' }
]

const statusOptions = computed(() => {
  return Object.keys(InvoiceStatus).map(key => ({
    title: InvoiceStatusLabels[InvoiceStatus[key]],
    value: InvoiceStatus[key]
  }))
})

const defaultItem = {
  invoiceNumber: '',
  customerId: null,
  issueDate: formatDateForInput(new Date()),
  dueDate: formatDateForInput(new Date(Date.now() + 30 * 24 * 60 * 60 * 1000)), // 30 days from now
  status: InvoiceStatus.DRAFT,
  lines: []
}

const defaultLine = {
  productId: null,
  quantity: 1,
  unitPrice: 0,
  taxRate: 0
}

const editedItem = ref({ ...defaultItem })
const itemToDelete = ref(null)

const rules = {
  required: value => !!value || value === 0 || 'Required.'
}

const getStatusLabel = (status) => InvoiceStatusLabels[status]
const getStatusColor = (status) => InvoiceStatusColors[status]

const loadInvoices = async () => {
  loading.value = true
  try {
    const response = await invoiceService.getAll()
    invoices.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading invoices')
  } finally {
    loading.value = false
  }
}

const loadCustomers = async () => {
  try {
    const response = await customerService.getAll()
    customers.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading customers')
  }
}

const loadProducts = async () => {
  try {
    const response = await productService.getAll()
    products.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading products')
  }
}

const openDialog = (item = null) => {
  if (item) {
    editedItem.value = {
      ...item,
      issueDate: formatDateForInput(item.issueDate),
      dueDate: formatDateForInput(item.dueDate),
      lines: item.lines || []
    }
  } else {
    editedItem.value = { ...defaultItem, lines: [] }
  }
  dialog.value = true
}

const closeDialog = () => {
  dialog.value = false
  editedItem.value = { ...defaultItem, lines: [] }
  form.value?.reset()
}

const addLine = () => {
  editedItem.value.lines.push({ ...defaultLine })
}

const removeLine = (index) => {
  editedItem.value.lines.splice(index, 1)
}

const updateLineFromProduct = (line) => {
  const product = products.value.find(p => p.id === line.productId)
  if (product) {
    line.unitPrice = product.unitPrice
    line.taxRate = product.taxRate
  }
}

const saveInvoice = async () => {
  const { valid: isValid } = await form.value.validate()
  if (!isValid || editedItem.value.lines.length === 0) return

  try {
    const invoiceData = {
      invoiceNumber: editedItem.value.invoiceNumber,
      customerId: editedItem.value.customerId,
      issueDate: new Date(editedItem.value.issueDate).toISOString(),
      dueDate: new Date(editedItem.value.dueDate).toISOString(),
      status: editedItem.value.status,
      lines: editedItem.value.lines
    }

    if (editedItem.value.id) {
      await invoiceService.update(editedItem.value.id, invoiceData)
      notificationStore.showSuccess('Invoice updated successfully')
    } else {
      await invoiceService.create(invoiceData)
      notificationStore.showSuccess('Invoice created successfully')
    }
    closeDialog()
    loadInvoices()
  } catch (error) {
    notificationStore.showError('Error saving invoice')
  }
}

const deleteInvoice = (item) => {
  itemToDelete.value = item
  deleteDialog.value = true
}

const confirmDelete = async () => {
  try {
    await invoiceService.delete(itemToDelete.value.id)
    notificationStore.showSuccess('Invoice deleted successfully')
    deleteDialog.value = false
    loadInvoices()
  } catch (error) {
    notificationStore.showError('Error deleting invoice')
  }
}

const viewInvoice = (item) => {
  router.push(`/invoices/${item.id}`)
}

onMounted(() => {
  loadInvoices()
  loadCustomers()
  loadProducts()
})
</script>
