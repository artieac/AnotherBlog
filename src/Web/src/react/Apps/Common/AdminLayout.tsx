import React from 'react';
import { Link, Outlet } from 'react-router-dom';

export const AdminLayout: React.FC = () => {
    return (
        <div className="min-h-screen bg-gray-100 flex flex-col">
            <nav className="bg-blue-800 text-white shadow-md">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex items-center justify-between h-16">
                        <div className="flex items-center">
                            <span className="text-xl font-bold mr-8">AnotherBlog Admin</span>
                            <div className="flex items-baseline space-x-4">
                                <Link to="/Admin/App/SiteInfo" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Site Info</Link>
                                <Link to="/Admin/App/ManageBlogs" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Blogs</Link>
                                <Link to="/Admin/App/ManageUsers" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Users</Link>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
            <main className="flex-grow">
                <div className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
                    <Outlet />
                </div>
            </main>
            <footer className="bg-white border-t border-gray-200 py-4 mt-auto">
                <div className="max-w-7xl mx-auto px-4 text-center text-gray-500 text-sm">
                    &copy; {new Date().getFullYear()} AnotherBlog Admin
                </div>
            </footer>
        </div>
    );
};
