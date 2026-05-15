import React from 'react';

export interface Column<T> {
    header: string;
    key: keyof T | string;
    render?: (item: T) => React.ReactNode;
    sortable?: boolean;
}

interface TableProps<T> {
    data: T[];
    columns: Column<T>[];
    keyField: keyof T;
    onSort?: (key: string) => void;
    sortConfig?: { key: string, direction: 'asc' | 'desc' } | null;
}

export const Table = <T,>({ data, columns, keyField, onSort, sortConfig }: TableProps<T>) => {
    return (
        <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                    <tr>
                        {columns.map((col, idx) => {
                            const isSorted = sortConfig?.key === col.key;
                            const isSortable = col.sortable && onSort;
                            
                            return (
                                <th
                                    key={idx}
                                    scope="col"
                                    className={`px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider ${isSortable ? 'cursor-pointer hover:bg-gray-100' : ''}`}
                                    onClick={() => isSortable && onSort(col.key as string)}
                                >
                                    <div className="flex items-center space-x-1">
                                        <span>{col.header}</span>
                                        {isSortable && (
                                            <span className="inline-flex flex-col">
                                                <svg 
                                                    className={`h-2 w-2 ${isSorted && sortConfig.direction === 'asc' ? 'text-blue-600' : 'text-gray-400'}`} 
                                                    fill="currentColor" viewBox="0 0 24 24"
                                                >
                                                    <path d="M12 4l-8 8h16l-8-8z" />
                                                </svg>
                                                <svg 
                                                    className={`h-2 w-2 ${isSorted && sortConfig.direction === 'desc' ? 'text-blue-600' : 'text-gray-400'}`} 
                                                    fill="currentColor" viewBox="0 0 24 24"
                                                >
                                                    <path d="M12 20l8-8H4l8 8z" />
                                                </svg>
                                            </span>
                                        )}
                                    </div>
                                </th>
                            );
                        })}
                    </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                    {data.map((item) => (
                        <tr key={String(item[keyField])}>
                            {columns.map((col, idx) => (
                                <td key={idx} className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                    {col.render ? col.render(item) : String(item[col.key as keyof T] ?? '')}
                                </td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};
