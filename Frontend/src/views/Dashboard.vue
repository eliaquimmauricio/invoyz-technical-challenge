<template>
  <v-container fluid class="pa-6">
    <v-row>
      <v-col cols="12">
        <h1 class="text-h3 mb-6">Dashboard</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12" md="4">
        <v-card color="primary" dark>
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-h6 mb-2">Customers</p>
                <p class="text-h3">{{ stats.customers }}</p>
              </div>
              <v-icon size="64">mdi-account-multiple</v-icon>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" to="/customers">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card color="success" dark>
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-h6 mb-2">Products</p>
                <p class="text-h3">{{ stats.products }}</p>
              </div>
              <v-icon size="64">mdi-package-variant</v-icon>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" to="/products">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card color="info" dark>
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-h6 mb-2">Invoices</p>
                <p class="text-h3">{{ stats.invoices }}</p>
              </div>
              <v-icon size="64">mdi-file-document</v-icon>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" to="/invoices">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-row class="mt-4">
      <v-col cols="12">
        <v-card>
          <v-card-title>Quick Actions</v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="primary" size="large" to="/customers">
                  <v-icon left>mdi-account-plus</v-icon>
                  New Customer
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="success" size="large" to="/products">
                  <v-icon left>mdi-package-variant-plus</v-icon>
                  New Product
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="info" size="large" to="/invoices">
                  <v-icon left>mdi-file-document-plus</v-icon>
                  New Invoice
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="secondary" size="large" to="/invoices">
                  <v-icon left>mdi-file-document-multiple</v-icon>
                  View Invoices
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { customerService } from '@/services/customerService'
import { productService } from '@/services/productService'
import { invoiceService } from '@/services/invoiceService'

const stats = ref({
  customers: 0,
  products: 0,
  invoices: 0
})

const loadStats = async () => {
  try {
    const [customersRes, productsRes, invoicesRes] = await Promise.all([
      customerService.getAll(),
      productService.getAll(),
      invoiceService.getAll()
    ])
    
    stats.value = {
      customers: customersRes.data.length,
      products: productsRes.data.length,
      invoices: invoicesRes.data.length
    }
  } catch (error) {
    console.error('Error loading stats:', error)
  }
}

onMounted(() => {
  loadStats()
})
</script>
