import React, { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchCurrentUser } from '@/redux/UserSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Navbar } from '@/components/layout/Navbar';

export const AdminLayout: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const { loggedInUser } = useSelector((state: RootState) => state.users);

    useEffect(() => {
        dispatch(fetchCurrentUser());
    }, [dispatch]);

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col">
            <Navbar user={loggedInUser} />
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
