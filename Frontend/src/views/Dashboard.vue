<template>
  <v-container fluid class="pa-6">
    <v-row>
      <v-col cols="12">
        <h1 class="text-h3 mb-6">Dashboard</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12" md="4">
        <v-card class="stat-card stat-card-primary">
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-caption text-medium-emphasis mb-1">Total Customers</p>
                <p class="text-h3 font-weight-bold">{{ stats.customers }}</p>
              </div>
              <div class="stat-icon">
                <v-icon size="48" color="primary">mdi-account-multiple</v-icon>
              </div>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" color="primary" to="/customers">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="stat-card stat-card-success">
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-caption text-medium-emphasis mb-1">Total Products</p>
                <p class="text-h3 font-weight-bold">{{ stats.products }}</p>
              </div>
              <div class="stat-icon">
                <v-icon size="48" color="success">mdi-package-variant</v-icon>
              </div>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" color="success" to="/products">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="stat-card stat-card-info">
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <p class="text-caption text-medium-emphasis mb-1">Total Invoices</p>
                <p class="text-h3 font-weight-bold">{{ stats.invoices }}</p>
              </div>
              <div class="stat-icon">
                <v-icon size="48" color="info">mdi-file-document</v-icon>
              </div>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" color="info" to="/invoices">View All</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-row class="mt-4">
      <v-col cols="12">
        <v-card>
          <v-card-title class="text-h5 font-weight-bold">Quick Actions</v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="success" size="large" to="/customers" class="action-btn" variant="outlined">
                  <v-icon left>mdi-account-plus</v-icon>
                  New Customer
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="success" size="large" to="/products" class="action-btn" variant="outlined">
                  <v-icon left>mdi-package-variant-plus</v-icon>
                  New Product
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="success" size="large" to="/invoices" class="action-btn" variant="outlined">
                  <v-icon left>mdi-file-document-plus</v-icon>
                  New Invoice
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn block color="success" size="large" to="/invoices" class="action-btn" variant="outlined">
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

<style scoped>
.stat-card {
  height: 100%;
  transition: transform 0.2s, box-shadow 0.2s;
  border: 1px solid rgba(99, 102, 241, 0.2);
}

.stat-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(99, 102, 241, 0.2);
}

.stat-icon {
  opacity: 0.15;
}

.action-btn {
  height: 56px !important;
}
</style>
