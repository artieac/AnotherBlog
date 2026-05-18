import React from 'react';
import { Link } from 'react-router-dom';
import { IUser } from '@/types/user.types';
import { UserMenu } from './UserMenu';

interface NavbarProps {
    user: IUser | null;
}

export const Navbar: React.FC<NavbarProps> = ({ user }) => {
    return (
        <nav className="bg-blue-800 text-white shadow-md">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex items-center justify-between h-16">
                    <div className="flex items-center">
                        <span className="text-xl font-bold mr-8">AnotherBlog Admin</span>
                        <div className="flex items-baseline space-x-4">
                            {user?.IsSiteAdministrator && (
                                <Link to="/Admin/App/SiteInfo" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Site Info</Link>
                            )}
                            <Link to="/Admin/App/ManageBlogs" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Blogs</Link>
                            {user?.IsSiteAdministrator && (
                                <Link to="/Admin/App/ManageUsers" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Manage Users</Link>
                            )}
                            <a href="/" className="px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700">View Site</a>
                        </div>
                    </div>
                    {user && <UserMenu user={user} />}
                </div>
            </div>
        </nav>
    );
};
