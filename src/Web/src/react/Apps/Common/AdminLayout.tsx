import React, { useEffect, useState } from 'react';
import { Link, Outlet } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchCurrentUser } from '@/redux/UserSlice';
import { RootState, AppDispatch } from '@/redux/store';

export const AdminLayout: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const { loggedInUser } = useSelector((state: RootState) => state.users);
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    useEffect(() => {
        dispatch(fetchCurrentUser());
    }, [dispatch]);

    useEffect(() => {
        const closeMenu = () => setIsMenuOpen(false);
        if (isMenuOpen) {
            window.addEventListener('click', closeMenu);
        }
        return () => window.removeEventListener('click', closeMenu);
    }, [isMenuOpen]);

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col">
            <nav className="bg-blue-800 text-white shadow-md">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex items-center justify-between h-16">
                        <div className="flex items-center">
                            <span className="text-xl font-bold mr-8">AnotherBlog Admin</span>
                            <div className="flex items-baseline space-x-4">
                                {loggedInUser?.IsSiteAdministrator && (
                                    <Link to="/Admin/App/SiteInfo" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Site Info</Link>
                                )}
                                <Link to="/Admin/App/ManageBlogs" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Blogs</Link>
                                {loggedInUser?.IsSiteAdministrator && (
                                    <Link to="/Admin/App/ManageUsers" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Users</Link>
                                )}
                                <a href="/" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">View Site</a>
                            </div>
                        </div>
                        <div className="relative">
                            {loggedInUser && (
                                <div>
                                    <button
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setIsMenuOpen(!isMenuOpen);
                                        }}
                                        className="flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700 focus:outline-none transition-colors"
                                    >
                                        <span>{loggedInUser.DisplayName}</span>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" className={`transition-transform ${isMenuOpen ? 'rotate-180' : ''}`}><path d="m6 9 6 6 6-6"/></svg>
                                    </button>
                                    {isMenuOpen && (
                                        <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 ring-1 ring-black ring-opacity-5 z-50 overflow-hidden">
                                            <a href="/User/Preferences" className="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" className="opacity-60"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
                                                Account
                                            </a>
                                            <form action="/User/Logout" method="post">
                                                <button type="submit" className="flex w-full items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-50 transition-colors border-t border-gray-100">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" className="opacity-60"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/></svg>
                                                    Sign Out
                                                </button>
                                            </form>
                                        </div>
                                    )}
                                </div>
                            )}
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
