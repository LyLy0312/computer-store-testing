import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import os
import uuid

plt.style.use('seaborn-v0_8-deep')
sns.set_palette("deep")

CHARTS_DIR = "charts"
if not os.path.exists(CHARTS_DIR):
    os.makedirs(CHARTS_DIR)

EXCEL_FILES = [
    'mainpage.xlsx',
    'registerpage.xlsx',
    'loginpage.xlsx',
    'confirmorderpage.xlsx',
    'cardpage.xlsx',
    'adminvieworderpage.xlsx',
    'adminviewfeedbackpage.xlsx',
    'adminproductpage.xlsx',
    'adminuserspage.xlsx',
    'aboutuspage.xlsx',
    'forgotpasswordpage.xlsx'
]

# Loading and combining data
def load_data():
    try:
        combined_df = pd.DataFrame()
        for file in EXCEL_FILES:
            if os.path.exists(file):
                df = pd.read_excel(file)
                page_name = os.path.splitext(file)[0].capitalize()
                df['Page'] = page_name
                print(f"{page_name} Columns:", df.columns.tolist())
                combined_df = pd.concat([combined_df, df], ignore_index=True)
            else:
                print(f"Warning: File {file} not found. Skipping.")
        if combined_df.empty:
            print("Error: No data loaded from any files.")
            return None
        combined_df.columns = combined_df.columns.str.replace(r'\s+', ' ', regex=True).str.strip()
        print("Combined DataFrame Columns:", combined_df.columns.tolist())
        return combined_df
    except Exception as e:
        print(f"Error loading files: {e}")
        return None

# Cleaning data
def clean_data(df):
    required_columns = ['Test Type', 'Pass/Fail', 'Page', 'Estimation (mins)', 'Priority', 'Area', 'TC ID', 'Title']
    missing_columns = [col for col in required_columns if col not in df.columns]
    if missing_columns:
        print(f"Error: Missing columns {missing_columns} in DataFrame")
        return None
    df['Estimation (mins)'] = pd.to_numeric(df['Estimation (mins)'], errors='coerce')
    df = df.dropna(subset=['Test Type', 'Pass/Fail', 'Page', 'Priority', 'Area'])
    return df

# Detailed Summary of Test Cases
def detailed_summary(df):
    print("\n=== Detailed Test Case Summary ===")
    total_tests = len(df)
    print(f"Total Test Cases: {total_tests}")
    
    # By Page
    print("\nTest Cases by Page:")
    page_counts = df['Page'].value_counts()
    for page, count in page_counts.items():
        pass_count = len(df[(df['Page'] == page) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Page'] == page) & (df['Pass/Fail'] == 'Fail')])
        print(f"- {page}: {count} cases (Pass: {pass_count}, Fail: {fail_count})")
    
    # By Test Type
    print("\nTest Cases by Test Type:")
    test_type_counts = df['Test Type'].value_counts()
    for test_type, count in test_type_counts.items():
        pass_count = len(df[(df['Test Type'] == test_type) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Test Type'] == test_type) & (df['Pass/Fail'] == 'Fail')])
        print(f"- {test_type}: {count} cases (Pass: {pass_count}, Fail: {fail_count})")
    
    # By Priority
    print("\nTest Cases by Priority:")
    priority_counts = df['Priority'].value_counts()
    for priority, count in priority_counts.items():
        pass_count = len(df[(df['Priority'] == priority) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Priority'] == priority) & (df['Pass/Fail'] == 'Fail')])
        print(f"- {priority}: {count} cases (Pass: {pass_count}, Fail: {fail_count})")
    
    # By Area
    print("\nTest Cases by Area:")
    area_counts = df['Area'].value_counts()
    for area, count in area_counts.items():
        pass_count = len(df[(df['Area'] == area) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Area'] == area) & (df['Pass/Fail'] == 'Fail')])
        print(f"- {area}: {count} cases (Pass: {pass_count}, Fail: {fail_count})")

# Plotting Test Type Distribution (Pie Chart)
def plot_test_type_distribution(df):
    test_type_counts = df['Test Type'].value_counts()
    plt.figure(figsize=(10, 7))
    plt.pie(test_type_counts, labels=test_type_counts.index, autopct='%1.1f%%', startangle=140)
    plt.title('Distribution of Test Types Across All Pages')
    plt.axis('equal')
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'test_type_distribution_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nTest Type Distribution Analysis:")
    total = test_type_counts.sum()
    for test_type, count in test_type_counts.items():
        percentage = count / total * 100
        pass_count = len(df[(df['Test Type'] == test_type) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Test Type'] == test_type) & (df['Pass/Fail'] == 'Fail')])
        page_breakdown = df[df['Test Type'] == test_type]['Page'].value_counts()
        print(f"- {test_type}: {count} cases ({percentage:.1f}%)")
        print(f"  Pass: {pass_count}, Fail: {fail_count}")
        print(f"  Pages:")
        for page, page_count in page_breakdown.items():
            print(f"    - {page}: {page_count} cases")
    print("Insight: Functional tests dominate, suggesting a focus on feature verification. Consider increasing performance and compatibility tests for broader coverage.")

# Plotting Pass/Fail Rate by Page (Bar Chart)
def plot_pass_fail_rate(df):
    pass_fail_counts = df.groupby(['Page', 'Pass/Fail']).size().unstack(fill_value=0)
    pass_fail_counts.plot(kind='bar', stacked=False, figsize=(12, 6))
    plt.title('Pass/Fail Rate by Page')
    plt.xlabel('Page')
    plt.ylabel('Number of Test Cases')
    plt.legend(title='Result')
    plt.xticks(rotation=45, ha='right')
    plt.tight_layout()
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'pass_fail_rate_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nPass/Fail Rate Analysis:")
    for page in pass_fail_counts.index:
        passes = pass_fail_counts.loc[page, 'Pass'] if 'Pass' in pass_fail_counts.columns else 0
        fails = pass_fail_counts.loc[page, 'Fail'] if 'Fail' in pass_fail_counts.columns else 0
        total = passes + fails
        failure_rate = (fails / total * 100) if total > 0 else 0
        test_types = df[df['Page'] == page]['Test Type'].value_counts()
        print(f"- {page}: {total} cases (Pass: {passes}, Fail: {fails}, Failure rate: {failure_rate:.1f}%)")
        print(f"  Test Types:")
        for test_type, count in test_types.items():
            print(f"    - {test_type}: {count} cases")
    print("Insight: High failure rates on specific pages (e.g., Loginpage for CAPTCHA) indicate areas needing focused debugging.")

# Plotting Test Duration by Page (Bar Chart)
def plot_test_duration(df):
    duration_by_page = df.groupby('Page')['Estimation (mins)'].sum()
    plt.figure(figsize=(12, 6))
    duration_by_page.plot(kind='bar', color=sns.color_palette("deep", n_colors=len(duration_by_page)))
    plt.title('Total Test Duration by Page')
    plt.xlabel('Page')
    plt.ylabel('Total Estimation (minutes)')
    plt.xticks(rotation=45, ha='right')
    plt.tight_layout()
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'test_duration_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nTest Duration Analysis:")
    for page, duration in duration_by_page.items():
        count = df[df['Page'] == page].shape[0]
        avg_duration = duration / count if count > 0 else 0
        test_types = df[df['Page'] == page]['Test Type'].value_counts()
        print(f"- {page}: {duration:.1f} minutes (Avg per case: {avg_duration:.2f} minutes, Total cases: {count})")
        print(f"  Test Types:")
        for test_type, type_count in test_types.items():
            type_duration = df[(df['Page'] == page) & (df['Test Type'] == test_type)]['Estimation (mins)'].sum()
            print(f"    - {test_type}: {type_count} cases, {type_duration:.1f} minutes")
    print("Insight: Pages with longer durations (e.g., Mainpage) may have more complex test cases or higher test counts.")

# Plotting Failure Distribution by Test Type (Bar Chart)
def plot_failure_by_test_type(df):
    failures = df[df['Pass/Fail'] == 'Fail'].groupby('Test Type').size()
    all_types = df['Test Type'].unique()
    failure_counts = pd.Series(0, index=all_types).add(failures, fill_value=0)
    plt.figure(figsize=(10, 6))
    failure_counts.plot(kind='bar', color='#d62728')
    plt.title('Failures by Test Type')
    plt.xlabel('Test Type')
    plt.ylabel('Number of Failures')
    plt.xticks(rotation=45, ha='right')
    plt.tight_layout()
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'failure_by_test_type_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nFailure by Test Type Analysis:")
    for test_type, count in failure_counts.items():
        total = df[df['Test Type'] == test_type].shape[0]
        rate = (count / total * 100) if total > 0 else 0
        page_breakdown = df[(df['Test Type'] == test_type) & (df['Pass/Fail'] == 'Fail')]['Page'].value_counts()
        print(f"- {test_type}: {int(count)} failures (Total cases: {total}, Failure rate: {rate:.1f}%)")
        if count > 0:
            print(f"  Failed Pages:")
            for page, page_count in page_breakdown.items():
                print(f"    - {page}: {page_count} failures")
    print("Insight: Functional test failures are prominent due to their volume. Review validation logic in these tests.")

# Plotting Priority Distribution (Pie Chart)
def plot_priority_distribution(df):
    priority_counts = df['Priority'].value_counts()
    plt.figure(figsize=(10, 7))
    plt.pie(priority_counts, labels=priority_counts.index, autopct='%1.1f%%', startangle=140)
    plt.title('Distribution of Test Case Priorities')
    plt.axis('equal')
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'priority_distribution_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nPriority Distribution Analysis:")
    total = priority_counts.sum()
    for priority, count in priority_counts.items():
        percentage = count / total * 100
        pass_count = len(df[(df['Priority'] == priority) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Priority'] == priority) & (df['Pass/Fail'] == 'Fail')])
        page_breakdown = df[df['Priority'] == priority]['Page'].value_counts()
        print(f"- {priority}: {count} cases ({percentage:.1f}%)")
        print(f"  Pass: {pass_count}, Fail: {fail_count}")
        print(f"  Pages:")
        for page, page_count in page_breakdown.items():
            print(f"    - {page}: {page_count} cases")
    print("Insight: High-priority tests are critical; ensure their stability. Low-priority failures may be cosmetic issues.")

# Plotting Test Case Count by Area (Stacked Bar Chart)
def plot_test_by_area(df):
    area_counts = df.groupby(['Page', 'Area']).size().unstack(fill_value=0)
    area_counts.plot(kind='bar', stacked=True, figsize=(14, 7))
    plt.title('Test Case Count by Area and Page')
    plt.xlabel('Page')
    plt.ylabel('Number of Test Cases')
    plt.legend(title='Area', bbox_to_anchor=(1.05, 1), loc='upper left')
    plt.xticks(rotation=45, ha='right')
    plt.tight_layout()
    chart_id = str(uuid.uuid4())
    plt.savefig(os.path.join(CHARTS_DIR, f'test_by_area_{chart_id}.png'))
    plt.show()
    plt.close()
    # Detailed Analysis
    print("\nTest Case by Area Analysis:")
    for page in area_counts.index:
        print(f"- {page}:")
        total_page = area_counts.loc[page].sum()
        pass_count = len(df[(df['Page'] == page) & (df['Pass/Fail'] == 'Pass')])
        fail_count = len(df[(df['Page'] == page) & (df['Pass/Fail'] == 'Fail')])
        print(f"  Total: {total_page} cases (Pass: {pass_count}, Fail: {fail_count})")
        for area, count in area_counts.loc[page].items():
            if count > 0:
                area_pass = len(df[(df['Page'] == page) & (df['Area'] == area) & (df['Pass/Fail'] == 'Pass')])
                area_fail = len(df[(df['Page'] == page) & (df['Area'] == area) & (df['Pass/Fail'] == 'Fail')])
                print(f"    - {area}: {count} cases (Pass: {area_pass}, Fail: {area_fail})")
    print("Insight: Areas like CAPTCHA Verification and Social Media Login have high test coverage but may have complex logic causing failures.")

# Analyzing Failures
def analyze_failures(df):
    failed_tests = df[df['Pass/Fail'] == 'Fail']
    print("\nDetailed Failure Analysis:")
    if failed_tests.empty:
        print("- No failures detected.")
    else:
        print(f"Total Failures: {len(failed_tests)}")
        print("\nFailures by Page:")
        page_fails = failed_tests['Page'].value_counts()
        for page, count in page_fails.items():
            print(f"- {page}: {count} failures")
        print("\nFailures by Test Type:")
        type_fails = failed_tests['Test Type'].value_counts()
        for test_type, count in type_fails.items():
            print(f"- {test_type}: {count} failures")
        print("\nIndividual Failed Test Cases:")
        for idx, row in failed_tests.iterrows():
            print(f"- TC ID: {row['TC ID']}, Title: {row['Title']}")
            print(f"  Page: {row['Page']}, Area: {row['Area']}, Test Type: {row['Test Type']}")
            print(f"  Expected: {row['Expected Results']}")
            print(f"  Actual: {row['Actual Results']}")
            print(f"  Recommendation: Investigate {row['Area'].lower()} logic, focusing on validation checks.")
    return len(failed_tests)

# Menu-driven interface
def display_menu():
    print("\n=== Test Case Analysis Menu ===")
    print("1. Test Type Distribution")
    print("2. Pass/Fail Rate by Page")
    print("3. Test Duration by Page")
    print("4. Failure Distribution by Test Type")
    print("5. Priority Distribution")
    print("6. Test Case Count by Area")
    print("7. All Charts")
    print("8. Exit")
    return input("Select an option (1-8): ")

# Main function
def main():
    df = load_data()
    if df is None:
        print("Failed to load data. Exiting.")
        return
    
    df = clean_data(df)
    if df is None:
        print("Failed to clean data. Exiting.")
        return
    
    # Calculate overall metrics
    total_tests = len(df)
    failed_count = analyze_failures(df)
    failure_rate = (failed_count / total_tests) * 100 if total_tests > 0 else 0
    print(f"\nOverall Summary:")
    print(f"- Total Test Cases: {total_tests}")
    print(f"- Failed Test Cases: {failed_count}")
    print(f"- Failure Rate: {failure_rate:.2f}%")
    print(f"- Average Test Duration: {df['Estimation (mins)'].mean():.2f} minutes per case")
    
    # Detailed summary
    detailed_summary(df)
    
    # Menu loop
    while True:
        choice = display_menu()
        if choice == '1':
            plot_test_type_distribution(df)
        elif choice == '2':
            plot_pass_fail_rate(df)
        elif choice == '3':
            plot_test_duration(df)
        elif choice == '4':
            plot_failure_by_test_type(df)
        elif choice == '5':
            plot_priority_distribution(df)
        elif choice == '6':
            plot_test_by_area(df)
        elif choice == '7':
            plot_test_type_distribution(df)
            plot_pass_fail_rate(df)
            plot_test_duration(df)
            plot_failure_by_test_type(df)
            plot_priority_distribution(df)
            plot_test_by_area(df)
        elif choice == '8':
            print("Exiting program.")
            break
        else:
            print("Invalid option. Please select 1-8.")

if __name__ == "__main__":
    main()