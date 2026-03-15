import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import 'vuetify/styles'

const vuetify = createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'invoyzDark',
    themes: {
      invoyzDark: {
        dark: true,
        colors: {
          primary: '#6366F1',        // Purple accent
          secondary: '#8B5CF6',      // Lighter purple
          accent: '#818CF8',         // Light purple/blue
          background: '#0F172A',     // Dark navy background
          surface: '#1E293B',        // Slightly lighter navy for cards
          'surface-bright': '#334155',
          'surface-light': '#475569',
          'surface-variant': '#1E293B',
          'on-surface-variant': '#CBD5E1',
          error: '#EF4444',
          info: '#3B82F6',
          success: '#10B981',
          warning: '#F59E0B',
          'on-primary': '#FFFFFF',
          'on-secondary': '#FFFFFF',
          'on-background': '#F1F5F9',
          'on-surface': '#E2E8F0',
        },
      },
      invoyzLight: {
        dark: false,
        colors: {
          primary: '#6366F1',
          secondary: '#8B5CF6',
          accent: '#818CF8',
          background: '#F8FAFC',
          surface: '#FFFFFF',
          'surface-bright': '#FFFFFF',
          'surface-light': '#F1F5F9',
          'surface-variant': '#F1F5F9',
          'on-surface-variant': '#475569',
          error: '#EF4444',
          info: '#3B82F6',
          success: '#10B981',
          warning: '#F59E0B',
        },
      },
    },
  },
})

export default vuetify
