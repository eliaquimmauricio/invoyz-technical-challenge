<template>
  <v-container fluid class="pa-6">
    <v-row>
      <v-col cols="12" class="d-flex justify-space-between align-center">
        <h1 class="text-h3">Products</h1>
        <v-btn color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Add Product
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
            :items="products"
            :search="search"
            :loading="loading"
            class="elevation-1"
          >
            <template v-slot:item.unitPrice="{ item }">
              {{ formatCurrency(item.unitPrice) }}
            </template>

            <template v-slot:item.taxRate="{ item }">
              {{ item.taxRate }}%
            </template>

            <template v-slot:item.createdAt="{ item }">
              {{ formatDate(item.createdAt) }}
            </template>

            <template v-slot:item.actions="{ item }">
              <v-icon
                size="small"
                class="mr-2"
                @click="openDialog(item)"
              >
                mdi-pencil
              </v-icon>
              <v-icon
                size="small"
                @click="deleteProduct(item)"
              >
                mdi-delete
              </v-icon>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- Product Dialog -->
    <v-dialog v-model="dialog" max-width="600px" persistent>
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ editedItem.id ? 'Edit Product' : 'New Product' }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="form" v-model="valid">
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="editedItem.name"
                  label="Name*"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-textarea
                  v-model="editedItem.description"
                  label="Description"
                  variant="outlined"
                  rows="3"
                ></v-textarea>
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  v-model.number="editedItem.unitPrice"
                  label="Unit Price*"
                  :rules="[rules.required, rules.number]"
                  variant="outlined"
                  type="number"
                  prefix="$"
                  step="0.01"
                ></v-text-field>
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  v-model.number="editedItem.taxRate"
                  label="Tax Rate (%)*"
                  :rules="[rules.required, rules.number]"
                  variant="outlined"
                  type="number"
                  suffix="%"
                  step="0.01"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="closeDialog">Cancel</v-btn>
          <v-btn color="primary" variant="flat" @click="saveProduct" :disabled="!valid">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card>
        <v-card-title class="text-h5">Confirm Delete</v-card-title>
        <v-card-text>Are you sure you want to delete this product?</v-card-text>
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
import { ref, onMounted } from 'vue'
import { productService } from '@/services/productService'
import { useNotificationStore } from '@/stores/notification'
import { formatDate, formatCurrency } from '@/utils/helpers'

const notificationStore = useNotificationStore()

const products = ref([])
const loading = ref(false)
const search = ref('')
const dialog = ref(false)
const deleteDialog = ref(false)
const valid = ref(false)
const form = ref(null)

const headers = [
  { title: 'Name', key: 'name', sortable: true },
  { title: 'Description', key: 'description', sortable: false },
  { title: 'Unit Price', key: 'unitPrice', sortable: true },
  { title: 'Tax Rate', key: 'taxRate', sortable: true },
  { title: 'Created', key: 'createdAt', sortable: true },
  { title: 'Actions', key: 'actions', sortable: false, align: 'end' }
]

const defaultItem = {
  name: '',
  description: '',
  unitPrice: 0,
  taxRate: 0
}

const editedItem = ref({ ...defaultItem })
const itemToDelete = ref(null)

const rules = {
  required: value => !!value || value === 0 || 'Required.',
  number: value => !isNaN(value) && value >= 0 || 'Must be a positive number.'
}

const loadProducts = async () => {
  loading.value = true
  try {
    const response = await productService.getAll()
    products.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading products')
  } finally {
    loading.value = false
  }
}

const openDialog = (item = null) => {
  if (item) {
    editedItem.value = { ...item }
  } else {
    editedItem.value = { ...defaultItem }
  }
  dialog.value = true
}

const closeDialog = () => {
  dialog.value = false
  editedItem.value = { ...defaultItem }
  form.value?.reset()
}

const saveProduct = async () => {
  const { valid: isValid } = await form.value.validate()
  if (!isValid) return

  try {
    if (editedItem.value.id) {
      await productService.update(editedItem.value.id, editedItem.value)
      notificationStore.showSuccess('Product updated successfully')
    } else {
      await productService.create(editedItem.value)
      notificationStore.showSuccess('Product created successfully')
    }
    closeDialog()
    loadProducts()
  } catch (error) {
    notificationStore.showError('Error saving product')
  }
}

const deleteProduct = (item) => {
  itemToDelete.value = item
  deleteDialog.value = true
}

const confirmDelete = async () => {
  try {
    await productService.delete(itemToDelete.value.id)
    notificationStore.showSuccess('Product deleted successfully')
    deleteDialog.value = false
    loadProducts()
  } catch (error) {
    notificationStore.showError('Error deleting product')
  }
}

onMounted(() => {
  loadProducts()
})
</script>
