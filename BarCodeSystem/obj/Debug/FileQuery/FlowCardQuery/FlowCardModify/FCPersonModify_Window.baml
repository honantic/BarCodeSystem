   M S B A M L   `   `   `  ����  BarCodeSystemP    JBarCodeSystem.FileQuery.FlowCardQuery.FlowCardModify.FCPersonModify_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowStyle$
  None=�5   
    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$. (/BarCodeSystem;component/Images/burn.ico��5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 人员修改窗口��5
   	   $	��400��6   $	��400��6&   .��5      � .+�5   
   �� 
����ح�$��4��6)   $��2��6:   $��10q�6N   6e   � 5   
   $��10q�6    � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6"   �� 5      $��40 �6    6.   �� 5      $��50 �6    6.   .+�5      �� $��修改报工人员��6   $��Center=�6&   $��Center=�6A   6`   &   System.Windows.Controls.DataGrid  5      -   5        $��datagrid_PersonInfo��5   M   $��1��6      CanUserAddRows .     
IsReadOnly .    RowHeaderWidth$ 30Q�6�   	   AutoGenerateColumns	 .  
   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$

 Auto 6�   ��6h   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 工序��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   ��  �� Path$ FCS_ProcessName�6`     5      $ 人员姓名��6)    67   �� $ FCS_PersonName�6a     5      $ 人员编号��6)    67   �� $ FCS_PersonCode�6a   5      � 5      $��2��6    � ColumnDefinitions 5      �� 5       6)   �� 5!      6)   �� 5"      6)   .+�5%      �� -   5        $��btn_ScanPerson��5%   (   $��扫描添加��6    �� 	ClickMode$ Release=�6[   $
��20 5q�6o   $��Center=�6~   $��Center=�6�   6�   �� 5'      -   5        $��btn_AddPerson��5'   (   $��手工添加��6   $ Release=�6Y   $��1��6m   $
��20 5q�6}   $��Center=�6�   $��Center=�6�   6�   �� 5)      -   5        $��btn_DeletePerson��5)   (   $��删除人员��6   $ Release=�6_   $
��20 5q�6s   $��Center=�6�   $��Center=�6�   $��2��6�   6�   5*      � 5,      $��3��6    5-      �� 5.      6)   �� 5/      6)   .+�52      �� -   5        $��
btn_Cancel��52   m   $��取消��6   $
��40 5q�6&   $��Center=�65   $��Center=�6P   $ Release=�6�   6�   �� 53      -   5        $��btn_Save��53   }   $��保存��6   $
��40 5q�6&   $��Center=�65   $��Center=�6P   $��1��6m   $ Release=�6�   6�   54      55      56      57      