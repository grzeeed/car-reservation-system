import { ReactNode } from 'react'
import { useAuth } from '@/contexts/AuthContext'
import Navbar from './Navbar'
import Sidebar from './Sidebar'

interface LayoutProps {
  children: ReactNode
}

export default function Layout({ children }: LayoutProps) {
  const { state } = useAuth()

  if (!state.isAuthenticated) {
    // For non-authenticated users, render children without layout
    return <>{children}</>
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="flex">
        <Sidebar />
        <main className="flex-1 p-8">
          <div className="max-w-7xl mx-auto">
            {children}
          </div>
        </main>
      </div>
    </div>
  )
}
