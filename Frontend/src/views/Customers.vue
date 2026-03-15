<template>
  <v-container fluid class="pa-6">
    <v-row>
      <v-col cols="12" class="d-flex justify-space-between align-center">
        <h1 class="text-h3">Customers</h1>
        <v-btn color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Add Customer
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
            :items="customers"
            :search="search"
            :loading="loading"
            class="elevation-1"
          >
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
                @click="deleteCustomer(item)"
              >
                mdi-delete
              </v-icon>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- Customer Dialog -->
    <v-dialog v-model="dialog" max-width="600px" persistent>
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ editedItem.id ? 'Edit Customer' : 'New Customer' }}</span>
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
                <v-text-field
                  v-model="editedItem.vatNumber"
                  label="VAT Number*"
                  :rules="[rules.required]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field
                  v-model="editedItem.email"
                  label="Email*"
                  :rules="[rules.required, rules.email]"
                  variant="outlined"
                ></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-textarea
                  v-model="editedItem.address"
                  label="Address*"
                  :rules="[rules.required]"
                  variant="outlined"
                  rows="3"
                ></v-textarea>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="closeDialog">Cancel</v-btn>
          <v-btn color="primary" variant="flat" @click="saveCustomer" :disabled="!valid">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card>
        <v-card-title class="text-h5">Confirm Delete</v-card-title>
        <v-card-text>Are you sure you want to delete this customer?</v-card-text>
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
import { customerService } from '@/services/customerService'
import { useNotificationStore } from '@/stores/notification'
import { formatDate } from '@/utils/helpers'

const notificationStore = useNotificationStore()

const customers = ref([])
const loading = ref(false)
const search = ref('')
const dialog = ref(false)
const deleteDialog = ref(false)
const valid = ref(false)
const form = ref(null)

const headers = [
  { title: 'Name', key: 'name', sortable: true },
  { title: 'VAT Number', key: 'vatNumber', sortable: true },
  { title: 'Email', key: 'email', sortable: true },
  { title: 'Address', key: 'address', sortable: false },
  { title: 'Created', key: 'createdAt', sortable: true },
  { title: 'Actions', key: 'actions', sortable: false, align: 'end' }
]

const defaultItem = {
  name: '',
  vatNumber: '',
  email: '',
  address: ''
}

const editedItem = ref({ ...defaultItem })
const itemToDelete = ref(null)

const rules = {
  required: value => !!value || 'Required.',
  email: value => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || 'Invalid email.'
  }
}

const loadCustomers = async () => {
  loading.value = true
  try {
    const response = await customerService.getAll()
    customers.value = response.data
  } catch (error) {
    notificationStore.showError('Error loading customers')
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

const saveCustomer = async () => {
  const { valid: isValid } = await form.value.validate()
  if (!isValid) return

  try {
    if (editedItem.value.id) {
      await customerService.update(editedItem.value.id, editedItem.value)
      notificationStore.showSuccess('Customer updated successfully')
    } else {
      await customerService.create(editedItem.value)
      notificationStore.showSuccess('Customer created successfully')
    }
    closeDialog()
    loadCustomers()
  } catch (error) {
    notificationStore.showError('Error saving customer')
  }
}

const deleteCustomer = (item) => {
  itemToDelete.value = item
  deleteDialog.value = true
}

const confirmDelete = async () => {
  try {
    await customerService.delete(itemToDelete.value.id)
    notificationStore.showSuccess('Customer deleted successfully')
    deleteDialog.value = false
    loadCustomers()
  } catch (error) {
    notificationStore.showError('Error deleting customer')
  }
}

onMounted(() => {
  loadCustomers()
})
</script>
